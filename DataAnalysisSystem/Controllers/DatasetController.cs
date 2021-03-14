using AutoMapper;
using DataAnalysisSystem.DataEntities;
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
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Authorize]
        [HttpGet]
        public IActionResult AddNewDataset(string notificationMessage = null, AddNewDatasetViewModel newDataset = null)
        {
            ViewData["Message"] = notificationMessage;
            ModelState.Clear();

            return View(newDataset);
        }

        [Authorize]
        [DisableRequestSizeLimit]
        [HttpPost]
        public IActionResult AddNewDataset(AddNewDatasetViewModel newDataset)
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

                newDataset.DatasetContent = _autoMapper.Map<DatasetContentViewModel>(_customSerializer.MapFileContentToObject(filePath, newDataset.AdditionalParameters));

                newDataset.InputFileFormat = modelDecision.FileExtension.ToLower();
                newDataset.InputFileName = newDataset.DatasetFile.FileName.Replace(newDataset.InputFileFormat, "");

                if (newDataset.DatasetContent == null)
                {
                    ModelState.AddModelError(string.Empty, "The file containing the dataset has a syntax error. Please verify the correctness of the uploaded file.");
                }

                _fileHelper.RemoveFileFromHardDrive(filePath);
            }

            return View(newDataset);
        }

        [Authorize]
        [DisableRequestSizeLimit]
        [HttpPost]
        public IActionResult SaveNewDataset(AddNewDatasetViewModel datasetToSave)
        {
            Dataset dataset = new Dataset
            {
                DatasetIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString(),
                DatasetName = datasetToSave.DatasetName,

                DateOfCreation = DateTime.UtcNow.ToString(),
                DateOfEdition = DateTime.UtcNow.ToString(),

                IsShared = false,
                AccessKey = "000",
            };
            dataset.DatasetContent = _autoMapper.Map<DatasetContent>(datasetToSave.DatasetContent);

            DatasetStatistics datasetStatistics = new DatasetStatistics
            {
                NumberOfColumns = datasetToSave.DatasetContent.NumberColumns.Count + datasetToSave.DatasetContent.StringColumns.Count,
                NumberOfRows = datasetToSave.DatasetContent.NumberColumns.Count != 0
                                ? datasetToSave.DatasetContent.NumberColumns.FirstOrDefault().AttributeValue.Count
                                : datasetToSave.DatasetContent.StringColumns.FirstOrDefault().AttributeValue.Count,

                NumberOfMissingValues = datasetToSave.DatasetContent.StringColumns.Select(z => z.AttributeValue.Where(s => String.IsNullOrWhiteSpace(s))).Count(),

                InputFileFormat = datasetToSave.InputFileFormat,
                InputFileName = datasetToSave.InputFileName
            };
            dataset.DatasetStatistics = datasetStatistics;

            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            _context.datasetRepository.AddDataset(dataset);
            _context.userRepository.AddDatasetToOwner(loggedUser.Id.ToString(), dataset.DatasetIdentificator);

            return RedirectToAction("AddNewDataset", "Dataset", new { notificationMessage = "The dataset has been successfully uploaded to the server." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserDatasets(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            var currentUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            List<Dataset> userDatasets = _context.datasetRepository.GetDatasetsById(currentUser.UserDatasets).ToList();

            List<DatasetOverallInformationViewModel> userDatasetsDTO = _autoMapper.Map<List<DatasetOverallInformationViewModel>>(userDatasets);

            return View(userDatasetsDTO);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DatasetDetails(string datasetIdentificator)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);

            DatasetDetailsViewModel datasetDetails = _autoMapper.Map<DatasetDetailsViewModel>(dataset);
            datasetDetails.DatasetContent = _autoMapper.Map<DatasetContentViewModel>(dataset.DatasetContent);
            datasetDetails.DatasetStatistics = _autoMapper.Map<DatasetDetailsStatisticsViewModel>(dataset.DatasetStatistics);

            return View(datasetDetails);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteDataset(string datasetIdentificator)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);

            if (dataset == null)
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            _context.datasetRepository.DeleteDataset(dataset.DatasetIdentificator);
            _context.userRepository.RemoveDatasetFromOwner(loggedUser.Id.ToString(), dataset.DatasetIdentificator);
            _context.userRepository.RemoveSharedDatasetsFromUsers(dataset.DatasetIdentificator);

            List<string> dataAnalysesId = _context.analysisRepository.GetAnalysesByDatasetId(dataset.DatasetIdentificator).Select(z=> z.AnalysisIdentificator).ToList();

            _context.analysisRepository.DeleteAnalyses(dataAnalysesId);
            _context.userRepository.RemoveAnalysesFromOwner(loggedUser.Id.ToString(), dataAnalysesId);
            _context.userRepository.RemoveSharedAnalysesFromUsers(dataAnalysesId);

            return RedirectToAction("MainAction", "UserSystemInteraction", new { notificationMessage = "The dataset and associated data analyses were successfully removed from the system." });
        }
    }
}