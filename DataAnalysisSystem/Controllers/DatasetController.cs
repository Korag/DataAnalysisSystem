using AutoMapper;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.Services;
using DataAnalysisSystem.Services.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DataAnalysisSystem.Controllers
{
    public class DatasetController : Controller
    {
        private const string DATASET_FOLDER_NAME = "resources/Datasets";

        private readonly ICodeGenerator _codeGenerator;
        private readonly IEmailProvider _emailProvider;
        private readonly IRegexComparatorChainFacade _regexComparator;
        private readonly IMimeTypeGuesser _mimeTypeGuesser;
        private readonly IFileHelper _fileHelper;

        private readonly ISerializerStrategy _serializerStrategy;

        private readonly CustomSerializer _customSerializer;

        private readonly IMapper _autoMapper;

        private readonly RepositoryContext _context;

        public DatasetController(
                                 RepositoryContext context,
                                 ICodeGenerator codeGenerator,
                                 IEmailProvider emailProvider,
                                 IMapper autoMapper,
                                 IRegexComparatorChainFacade regexComparator,
                                 IMimeTypeGuesser mimeTypeGuesser,
                                 IFileHelper fileHelper)
        {

            this._context = context;

            this._codeGenerator = codeGenerator;
            this._emailProvider = emailProvider;
            this._regexComparator = regexComparator;
            this._mimeTypeGuesser = mimeTypeGuesser;
            this._fileHelper = fileHelper;

            this._customSerializer = CustomSerializer.GetInstance(_serializerStrategy);

            this._autoMapper = autoMapper;
        }

        //Add block when dataTable is displayed and add form to action MapDatasetToObject
        [Authorize]
        [HttpGet]
        public IActionResult AddNewDataset(string notificationMessage = null, AddNewDatasetViewModel newDataset = null)
        {
            ViewData["notificationMessage"] = notificationMessage;
            ModelState.Clear();

            return View(newDataset);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewDataset(AddNewDatasetViewModel newDataset)
        {
            if (ModelState.IsValid)
            {
                IRegexComparatorChainFacade comparatorFacade = new RegexComparatorChainFacade();
                IMimeTypeGuesser mimeTypeGuesser = new MimeTypeGuesser();

                byte[] datasetBinaryFile = _fileHelper.ConvertIFormFileToByteArray(newDataset.DatasetFile);
                string filePath = _fileHelper.SaveFileOnHardDrive(newDataset.DatasetFile, DATASET_FOLDER_NAME);

                string mimeType = _mimeTypeGuesser.GetMimeTypeFromByteArray(datasetBinaryFile, newDataset.DatasetFile.FileName);

                RegexDecisionDTO modelDecision = new RegexDecisionDTO()
                {
                    FileExtension = _fileHelper.ExtractExtensionFromFilePath(newDataset.DatasetFile.FileName),
                    MimeType = mimeType
                };

                ISerializerStrategy chosenStrategy = comparatorFacade.GetSerializerStrategyBasedOnFileType(modelDecision);

                if (chosenStrategy == null)
                {
                    ModelState.AddModelError(string.Empty, "Incorrect format of the uploaded file. Only the following formats are supported: .csv, .json, .xml, .xls, .xlsx.\n");
                    ModelState.AddModelError(string.Empty, "If you have entered a file in the correct format and receive this error, it means that the indicated file contains syntax errors such as omitted values.");

                    _fileHelper.RemoveFileFromHardDrive(filePath);

                    return View(newDataset);
                }

                _customSerializer.ChangeStrategy(chosenStrategy);
               
                newDataset.DatasetContent = _customSerializer.MapFileContentToObject(filePath, newDataset.AdditionalParameters);
               
                newDataset.InputFileFormat = modelDecision.FileExtension.ToLower();
                newDataset.InputFileName = newDataset.DatasetFile.FileName.Replace(newDataset.InputFileFormat, "");

                _fileHelper.RemoveFileFromHardDrive(filePath);
            }

            return View(newDataset);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SaveNewDataset(AddNewDatasetViewModel datasetToSave)
        {
            //Save ICollection<DatasetColumnAbstract> DatasetContent to database and redirect to empty AddNewDataset with notification

            return RedirectToAction("AddNewDataset", "Dataset", new { notificationMessage = "The dataset has been successfully uploaded to the server." });
        }
    }
}
