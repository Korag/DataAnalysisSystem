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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnalysisSystem.Controllers
{
    public class DatasetController : Controller
    {
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
                
                //Something is wrong - FIX IT
                string datasetStringFile = _fileHelper.ConvertIFormFileToString(newDataset.DatasetFile);

                string mimeType = _mimeTypeGuesser.GetMimeTypeFromByteArray(datasetBinaryFile, newDataset.DatasetFile.FileName);

                RegexDecisionDTO modelDecision = new RegexDecisionDTO()
                {
                    FileExtension = _fileHelper.ExtractExtensionFromFilePath(newDataset.DatasetFile.FileName),
                    MimeType = mimeType
                };

                ISerializerStrategy chosenStrategy = comparatorFacade.GetSerializerStrategyBasedOnFileType(modelDecision);

                if (chosenStrategy == null)
                {
                    ModelState.AddModelError(string.Empty, "Incorrect format of the uploaded file. Only the following formats are supported: .csv, .json, .xml, .xls, .xlsx.");

                    return View(newDataset);
                }

                _customSerializer.ChangeStrategy(chosenStrategy);

                if (chosenStrategy.GetType() == typeof(CsvSerializerStrategy))
                {
                    //If strategy is CSV then ask question to user about Delimiter -> form

                    AddDelimiterInformationViewModel delimiterInformation = new AddDelimiterInformationViewModel()
                    {
                        DatasetName = newDataset.DatasetName,

                        DatasetContentByteArray = datasetBinaryFile,
                        DatasetContentString = datasetStringFile,
                        DatasetFile = newDataset.DatasetFile,

                        InputFileName = newDataset.DatasetFile.FileName,
                        InputFileFormat = modelDecision.FileExtension,

                        CsvDelimiter = ";"
                    };

                    return View("GetDatasetDelimiter");

                    //return RedirectToAction("GetDatasetDelimiter", "Dataset", new { delimiterInformation, notificationMessage = "A .csv file was selected. Please indicate the delimiter character." });
                }
                else
                {
                    MapDatasetToObjectViewModel mapDataset = new MapDatasetToObjectViewModel()
                    {
                        DatasetName = newDataset.DatasetName,
                        DatasetContentByteArray = datasetBinaryFile,
                        DatasetContentString = datasetStringFile,
                        InputFileName = newDataset.DatasetFile.FileName,
                        InputFileFormat = modelDecision.FileExtension
                    };
                  
                    return RedirectToAction("MapDatasetToObjectModel", "Dataset", new { notificationMessage = mapDataset });
                }
            }

            return View(newDataset);
        }

        //[Authorize]
        //[HttpGet]
        //public IActionResult GetDatasetDelimiter(AddDelimiterInformationViewModel delimiterInformation, string notificationMessage = null)
        //{
        //    //If strategy is CSV then ask question to user about Delimiter -> form
        //    ViewData["notificationMessage"] = notificationMessage;

        //    return View(delimiterInformation);
        //}

        //TO DO: Add View with spinner
        [Authorize]
        [HttpPost]
        public IActionResult GetDatasetDelimiter(AddDelimiterInformationViewModel delimiterInformation)
        {
            //If strategy is CSV then ask question to user about Delimiter -> form

            if (ModelState.IsValid)
            {
                MapDatasetToObjectViewModel mapDataset = _autoMapper.Map<MapDatasetToObjectViewModel>(delimiterInformation);

                return RedirectToAction("MapDatasetToObjectModel", "Dataset", new { datasetToMap = mapDataset, csvDelimiter = delimiterInformation.CsvDelimiter });
            }

            return View(delimiterInformation);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MapDatasetToObject(MapDatasetToObjectViewModel datasetToMap, string csvDelimiter = null)
        {
            //Parse file content by tool 
            //Return to AddNewDataset View and display loaded dataset (in object model)
            _customSerializer.MapFileContentToObject(datasetToMap.DatasetContentString);

            //if()
            //{

            //}
            
            //TO DO: Add mapAdditionalParameters class with delimiter and pass to strategy. User in CsvSerializerStrategy.
            //Create AddNewDatasetViewModel and pass through Redirect method

            return RedirectToAction("AddNewDataset", "Dataset", new { notificationMessage = "The dataset has been loaded." });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveDataset(AddNewDatasetViewModel datasetToSave)
        {
            //Save ICollection<DatasetColumnAbstract> DatasetContent to database and redirect to empty AddNewDataset with notification

            return RedirectToAction("AddNewDataset", "Dataset", new { notificationMessage = "The dataset has been successfully uploaded to the server." });
        }
    }
}
