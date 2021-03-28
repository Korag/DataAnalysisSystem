using AutoMapper;
using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Extensions;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.Services;
using DataAnalysisSystem.Services.DesignPatterns.AdapterDesignPattern;
using DataAnalysisSystem.Services.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.AdapterDesignPattern;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnalysisSystem.Controllers
{
    public class DatasetController : Controller
    {
        private const string DATASET_FOLDER_NAME = "resources/Datasets";
        private const string DATASET_CSV_FILE_EXTENSION = ".csv";
        private const string DATASET_JSON_FILE_EXTENSION = ".json";
        private const string DATASET_XLSX_FILE_EXTENSION = ".xlsx";
        private const string DATASET_XML_FILE_EXTENSION = ".xml";

        private const string DATASET_CSV_FORMAT_TYPE = "csv";
        private const string DATASET_JSON_FORMAT_TYPE = "json";
        private const string DATASET_XLSX_FORMAT_TYPE = "xlsx";
        private const string DATASET_XML_FORMAT_TYPE = "xml";

        private readonly ICodeGenerator _codeGenerator;
        private readonly IEmailProvider _emailProvider;
        private readonly IRegexComparatorChainFacade _regexComparator;
        private readonly IMimeTypeGuesser _mimeTypeGuesser;
        private readonly IFileHelper _fileHelper;

        private readonly IWebHostEnvironment _environment;

        private readonly CustomSerializer _customSerializer;

        private readonly IMapper _autoMapper;

        private readonly RepositoryContext _context;

        //All actions OK
        public DatasetController(RepositoryContext context,
                                 CustomSerializer customSerializer,
                                 IWebHostEnvironment environment,
                                 ICodeGenerator codeGenerator,
                                 IEmailProvider emailProvider,
                                 IMapper autoMapper,
                                 IRegexComparatorChainFacade regexComparator,
                                 IMimeTypeGuesser mimeTypeGuesser,
                                 
                                 IFileHelper fileHelper)
        {

            this._context = context;
            this._environment = environment;

            this._codeGenerator = codeGenerator;

            this._emailProvider = emailProvider;
            this._regexComparator = regexComparator;
            this._mimeTypeGuesser = mimeTypeGuesser;
            this._fileHelper = fileHelper;

            this._customSerializer = customSerializer;

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
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (loggedUser.SharedDatasetsToUser.Contains(datasetIdentificator))
            {
                return RedirectToAction("SharedDatasetDetails", "Dataset");
            }
            else if (!loggedUser.UserDatasets.Contains(datasetIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            DatasetDetailsViewModel datasetDetails = _autoMapper.Map<DatasetDetailsViewModel>(dataset);
            datasetDetails.DatasetContent = _autoMapper.Map<DatasetContentViewModel>(dataset.DatasetContent);
            datasetDetails.DatasetStatistics = _autoMapper.Map<DatasetDetailsStatisticsViewModel>(dataset.DatasetStatistics);

            datasetDetails.DatasetStatistics.AttributesDistribution = JsonConvert.SerializeObject(new int[] { datasetDetails.DatasetContent.NumberColumns.Count, datasetDetails.DatasetContent.StringColumns.Count });
            datasetDetails.DatasetStatistics.MissingValuePercentage = JsonConvert.SerializeObject(dataset.DatasetStatistics.NumberOfMissingValues / ((dataset.DatasetStatistics.NumberOfColumns * dataset.DatasetStatistics.NumberOfRows) - dataset.DatasetStatistics.NumberOfMissingValues));

            return View(datasetDetails);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteDataset(string datasetIdentificator)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (dataset == null || !loggedUser.UserDatasets.Contains(datasetIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            _context.datasetRepository.DeleteDataset(dataset.DatasetIdentificator);
            _context.userRepository.RemoveDatasetFromOwner(loggedUser.Id.ToString(), dataset.DatasetIdentificator);
            _context.userRepository.RemoveSharedDatasetsFromUsers(dataset.DatasetIdentificator);

            List<string> dataAnalysesId = _context.analysisRepository.GetAnalysesByDatasetId(dataset.DatasetIdentificator).Select(z => z.AnalysisIdentificator).ToList();

            _context.analysisRepository.DeleteAnalyses(dataAnalysesId);
            _context.userRepository.RemoveAnalysesFromOwner(loggedUser.Id.ToString(), dataAnalysesId);
            _context.userRepository.RemoveSharedAnalysesFromUsers(dataAnalysesId);

            return RedirectToAction("MainAction", "UserSystemInteraction", new { notificationMessage = "The dataset and associated data analyses were successfully removed from the system." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserSharedDatasets(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            List<Dataset> userDatasets = _context.datasetRepository.GetDatasetsById(loggedUser.UserDatasets).ToList();

            ShareDatasetActionViewModel sharedDatasetInfo = new ShareDatasetActionViewModel();

            foreach (var dataset in userDatasets)
            {
                if (dataset.IsShared)
                {
                    SharedDatasetByOwnerViewModel sharedDataset = _autoMapper.Map<SharedDatasetByOwnerViewModel>(dataset);
                    sharedDataset = _autoMapper.Map<DatasetStatistics, SharedDatasetByOwnerViewModel>(dataset.DatasetStatistics, sharedDataset);

                    string urlToAction = Url.GenerateLinkToSharedDataset(sharedDataset.AccessKey, Request.Scheme);
                    sharedDataset.UrlToAction = urlToAction;
                    //string iconURL = Path.Combine(_environment.WebRootPath, "Images") + $@"\qrCodeIcon.bmp";
                    //sharedDataset.AccessQRCode = _qrCodeGenerator.GenerateQRCode(payload, iconURL);

                    sharedDatasetInfo.SharedDatasets.Add(sharedDataset);
                }
                else
                {
                    NotSharedDatasetViewModel notSharedDataset = _autoMapper.Map<NotSharedDatasetViewModel>(dataset);
                    notSharedDataset = _autoMapper.Map<DatasetStatistics, NotSharedDatasetViewModel>(dataset.DatasetStatistics, notSharedDataset);

                    sharedDatasetInfo.NotSharedDatasets.Add(notSharedDataset);
                }
            }

            return View(sharedDatasetInfo);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ShareDataset(string datasetIdentificator)
        {
            Dataset datasetToShare = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (datasetToShare == null || !loggedUser.UserDatasets.Contains(datasetIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            datasetToShare.IsShared = true;
            datasetToShare.AccessKey = _codeGenerator.GenerateAccessKey(8);

            _context.datasetRepository.UpdateDataset(datasetToShare);

            return RedirectToAction("UserSharedDatasets", "Dataset", new { notificationMessage = "A data set has been made available." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult StopSharingDataset(string datasetIdentificator)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            _context.userRepository.RemoveSharedDatasetsFromUsers(datasetIdentificator);

            List<string> dataAnalysesId = _context.analysisRepository.GetAnalysesByDatasetId(dataset.DatasetIdentificator).Select(z => z.AnalysisIdentificator).ToList();
            _context.userRepository.RemoveSharedAnalysesFromUsers(dataAnalysesId);

            dataset.IsShared = false;
            dataset.AccessKey = "000";
            _context.datasetRepository.UpdateDataset(dataset);

            return RedirectToAction("UserSharedDatasets", "Dataset", new { notificationMessage = "The selected dataset is no longer shared with other system users." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult ExportDataset(string datasetIdentificator)
        {
            Dataset datasetToExport = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (datasetToExport == null && (!loggedUser.UserDatasets.Contains(datasetIdentificator) || !loggedUser.SharedDatasetsToUser.Contains(datasetIdentificator)))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            ExportDatasetViewModel exportDataset = _autoMapper.Map<ExportDatasetViewModel>(datasetToExport);
            exportDataset.DatasetContent = _autoMapper.Map<DatasetContentViewModel>(datasetToExport.DatasetContent);

            #region ToCheck
            //var interfaceType = typeof(ISerializerStrategy);
            //var subclasses = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(s => s.GetTypes())
            //    .Where(p => interfaceType.IsAssignableFrom(p) && !p.IsInterface);

            //foreach (var strategy in subclasses)
            //{
            //    _customSerializer.ChangeStrategy((ISerializerStrategy)strategy.GetElementType());
            //    _customSerializer.ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent);
            //}

            //Task<string> csvSerializerTask = Task<string>.Factory.StartNew(() => {return new CustomSerializer(new CsvSerializerStrategy()).ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent); });
            //Task<string> jsonSerializerTask = Task<string>.Factory.StartNew(() => {return new CustomSerializer(new JsonSerializerStrategy()).ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent); });
            //Task<string> xlsxSerializerTask = Task<string>.Factory.StartNew(() => { return new CustomSerializer(new XlsxSerializerStrategy()).ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent); });
            //Task<string> xmlSerializerTask = Task<string>.Factory.StartNew(() => { return new CustomSerializer(new XmlSerializerStrategy()).ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent); });

            //exportDataset.DatasetContentFormatCSV = csvSerializerTask.Result;
            //exportDataset.DatasetContentFormatJSON = jsonSerializerTask.Result;
            //exportDataset.DatasetContentFormatXLSX = xlsxSerializerTask.Result;
            //exportDataset.DatasetContentFormatXML = xmlSerializerTask.Result;
            #endregion

            _customSerializer.ChangeStrategy(new CsvSerializerStrategy());
            exportDataset.DatasetContentFormatCSV = _customSerializer.ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent);

            _customSerializer.ChangeStrategy(new JsonSerializerStrategy());
            exportDataset.DatasetContentFormatJSON = _customSerializer.ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent);

            _customSerializer.ChangeStrategy(new XmlSerializerStrategy());
            exportDataset.DatasetContentFormatXML = _customSerializer.ConvertFromObjectToSpecificFile(datasetToExport.DatasetContent);

            return View(exportDataset);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DownloadDatasetInSpecificFormat(string datasetIdentificator, string formatType)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (dataset == null && (!loggedUser.UserDatasets.Contains(datasetIdentificator) || !loggedUser.SharedDatasetsToUser.Contains(datasetIdentificator)))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            string filePath = "";
            string datasetContentFormat = "";
            string fileDownloadName = dataset.DatasetName;

            switch (formatType)
            {
                case DATASET_CSV_FORMAT_TYPE:
                    _customSerializer.ChangeStrategy(new CsvSerializerStrategy());
                    datasetContentFormat = _customSerializer.ConvertFromObjectToSpecificFile(dataset.DatasetContent);
                    filePath = _fileHelper.SaveStringToFileLocatedOnHardDrive(datasetContentFormat, dataset.DatasetName, DATASET_CSV_FILE_EXTENSION, DATASET_FOLDER_NAME);
                    fileDownloadName += DATASET_CSV_FILE_EXTENSION;
                    break;
                case DATASET_JSON_FORMAT_TYPE:
                    _customSerializer.ChangeStrategy(new JsonSerializerStrategy());
                    datasetContentFormat = _customSerializer.ConvertFromObjectToSpecificFile(dataset.DatasetContent);
                    filePath = _fileHelper.SaveStringToFileLocatedOnHardDrive(datasetContentFormat, dataset.DatasetName, DATASET_JSON_FILE_EXTENSION, DATASET_FOLDER_NAME);
                    fileDownloadName += DATASET_JSON_FILE_EXTENSION;
                    break;
                case DATASET_XLSX_FORMAT_TYPE:
                    _customSerializer.ChangeStrategy(new XlsxSerializerStrategy());
                    IXlsxSerializerAdapter xlsxAdapter = new XlsxSerializerAdapter(_codeGenerator, _environment);
                    filePath = xlsxAdapter.SaveStringToFileLocatedOnHardDrive(dataset.DatasetContent, dataset.DatasetName, DATASET_XLSX_FILE_EXTENSION, DATASET_FOLDER_NAME);
                    fileDownloadName += DATASET_XLSX_FILE_EXTENSION;
                    break;
                case DATASET_XML_FORMAT_TYPE:
                    _customSerializer.ChangeStrategy(new XmlSerializerStrategy());
                    datasetContentFormat = _customSerializer.ConvertFromObjectToSpecificFile(dataset.DatasetContent);
                    filePath = _fileHelper.SaveStringToFileLocatedOnHardDrive(datasetContentFormat, dataset.DatasetName, DATASET_XML_FILE_EXTENSION, DATASET_FOLDER_NAME);
                    fileDownloadName += DATASET_XML_FILE_EXTENSION;
                    break;
            }

            var fs = new FileStream(filePath, FileMode.Open);

            Response.OnCompleted(() => {
                _fileHelper.RemoveFileFromHardDrive(filePath);
                return Task.CompletedTask;
            });

            return File(fs, "application/octet-stream", fileDownloadName);
        }

        public string GetDatasetOwnerName(string datasetIdentificator)
        {
            IdentityProviderUser datasetOwner = _context.userRepository.GetDatasetOwnerByDatasetId(datasetIdentificator);

            if (datasetOwner == null)
            {
                return "not found";
            }

            return datasetOwner.FirstName + " " + datasetOwner.LastName;
        }

        public SharedDatasetsBrowserViewModel GetDatasetsSharedToLoggedUser()
        {
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            List<Dataset> datasetShared = _context.datasetRepository.GetDatasetsById(loggedUser.SharedDatasetsToUser).ToList();

            SharedDatasetsBrowserViewModel datasetSharedVm = new SharedDatasetsBrowserViewModel();
            
            datasetSharedVm.SharedDatasets = _autoMapper.Map<List<SharedDatasetByCollabViewModel>>(datasetShared).ToList();
            List<DatasetStatistics> datasetStatistics = datasetShared.Select(z => z.DatasetStatistics).ToList();

            for (int i = 0; i < datasetSharedVm.SharedDatasets.Count; i++)
            {
                datasetSharedVm.SharedDatasets[i] = _autoMapper.Map<DatasetStatistics, SharedDatasetByCollabViewModel>(datasetStatistics[i], datasetSharedVm.SharedDatasets[i]);
            }
            datasetSharedVm.SharedDatasets.ToList().ForEach(z => z.OwnerName = GetDatasetOwnerName(z.DatasetIdentificator));

            return datasetSharedVm;
        }

        [Authorize]
        [HttpGet]
        public IActionResult SharedDatasetsBrowser(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            SharedDatasetsBrowserViewModel datasetBrowser = GetDatasetsSharedToLoggedUser();

            return View(datasetBrowser);
        }

        [Authorize]
        [HttpPost]
        [ActionName("SharedDatasetsBrowser")]
        public IActionResult SharedDatasetsBrowserPost(string newSharedDatasetAccessKey)
        {
            #region Legacy
            //Dataset datasetShared = _context.datasetRepository.GetDatasetByAccessKey(datasetBrowser.NewSharedDatasetAccessKey);

            //if (ModelState.IsValid && datasetShared != null)
            //{
            //    var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            //    if (loggedUser.UserDatasets.Contains(datasetShared.DatasetIdentificator))
            //    {
            //        datasetBrowser = GetDatasetsSharedToLoggedUser();
            //        ModelState.AddModelError(string.Empty, "You are the owner of this data set.");

            //        return View(datasetBrowser);
            //    }
            //    if(loggedUser.SharedDatasetsToUser.Contains(datasetShared.DatasetIdentificator))
            //    {
            //        datasetBrowser = GetDatasetsSharedToLoggedUser();
            //        ModelState.AddModelError(string.Empty, "You already have access to this dataset.");

            //        return View(datasetBrowser);
            //    }

            //    loggedUser.SharedDatasetsToUser.Add(datasetShared.DatasetIdentificator);
            //    _context.userRepository.UpdateUser(loggedUser);

            //    return RedirectToAction("SharedDatasetsBrowser", "Dataset", new { notificationMessage = "You have successfully gained access to a shared dataset." });
            //}
            //else
            //{
            //    datasetBrowser = GetDatasetsSharedToLoggedUser();
            //    ModelState.AddModelError(string.Empty, "Invalid access key.");

            //    return View(datasetBrowser);
            //}
            #endregion

            return RedirectToAction("GainAccessToSharedDataset", new { datasetAccessKey = newSharedDatasetAccessKey });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GainAccessToSharedDataset(string datasetAccessKey)
        {
            Dataset datasetShared = _context.datasetRepository.GetDatasetByAccessKey(datasetAccessKey);
      
            if (datasetShared != null)
            {
                var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

                if (loggedUser.UserDatasets.Contains(datasetShared.DatasetIdentificator))
                {
                    ModelState.AddModelError(string.Empty, "You are the owner of this data set.");
                    SharedDatasetsBrowserViewModel datasetBrowser = GetDatasetsSharedToLoggedUser();

                    return View("SharedDatasetsBrowser", datasetBrowser);
                }
                if (loggedUser.SharedDatasetsToUser.Contains(datasetShared.DatasetIdentificator))
                {
                    ModelState.AddModelError(string.Empty, "You already have access to this dataset.");
                    SharedDatasetsBrowserViewModel datasetBrowser = GetDatasetsSharedToLoggedUser();

                    return View("SharedDatasetsBrowser", datasetBrowser);
                }

                loggedUser.SharedDatasetsToUser.Add(datasetShared.DatasetIdentificator);
                _context.userRepository.UpdateUser(loggedUser);

                return RedirectToAction("SharedDatasetsBrowser", "Dataset", new { notificationMessage = "Access was gained to a shared dataset." });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid access key.");
                SharedDatasetsBrowserViewModel datasetBrowser = GetDatasetsSharedToLoggedUser();

                return View("SharedDatasetsBrowser", datasetBrowser);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult SharedDatasetDetails(string datasetIdentificator)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (loggedUser.UserDatasets.Contains(datasetIdentificator))
            {
                return RedirectToAction("DatasetDetails", "Dataset", new { datasetIdentificator = datasetIdentificator });
            }
            else if (!loggedUser.SharedDatasetsToUser.Contains(datasetIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            DatasetDetailsViewModel datasetDetails = _autoMapper.Map<DatasetDetailsViewModel>(dataset);
            datasetDetails.DatasetContent = _autoMapper.Map<DatasetContentViewModel>(dataset.DatasetContent);
            datasetDetails.DatasetStatistics = _autoMapper.Map<DatasetDetailsStatisticsViewModel>(dataset.DatasetStatistics);

            datasetDetails.DatasetStatistics.AttributesDistribution = JsonConvert.SerializeObject(new int[] { datasetDetails.DatasetContent.NumberColumns.Count, datasetDetails.DatasetContent.StringColumns.Count });
            datasetDetails.DatasetStatistics.MissingValuePercentage = JsonConvert.SerializeObject(dataset.DatasetStatistics.NumberOfMissingValues / ((dataset.DatasetStatistics.NumberOfColumns * dataset.DatasetStatistics.NumberOfRows) - dataset.DatasetStatistics.NumberOfMissingValues));

            return View(datasetDetails);
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditDataset(string datasetIdentificator)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (dataset == null && (!loggedUser.UserDatasets.Contains(datasetIdentificator) || !loggedUser.SharedDatasetsToUser.Contains(datasetIdentificator)))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            EditDatasetViewModel datasetToEdit = _autoMapper.Map<EditDatasetViewModel>(dataset);
            datasetToEdit.DatasetContent = _autoMapper.Map<EditDatasetContentViewModel>(dataset.DatasetContent);

            if (dataset.DatasetContent.StringColumns.Count != 0)
            {
                datasetToEdit.DatasetContent.RowsToDelete = new bool[dataset.DatasetContent.StringColumns.FirstOrDefault().AttributeValue.Count()];
            }
            else
            {
                datasetToEdit.DatasetContent.RowsToDelete = new bool[dataset.DatasetContent.NumberColumns.FirstOrDefault().AttributeValue.Count()];
            }

            return View(datasetToEdit);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditDataset(EditDatasetViewModel datasetToEdit)
        {
            if (ModelState.IsValid)
            {
                Dataset dataset = _context.datasetRepository.GetDatasetById(datasetToEdit.DatasetIdentificator);
                var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

                if (dataset == null && (!loggedUser.UserDatasets.Contains(datasetToEdit.DatasetIdentificator) || !loggedUser.SharedDatasetsToUser.Contains(datasetToEdit.DatasetIdentificator)))
                {
                    return RedirectToAction("MainAction", "UserSystemInteraction");
                }

                dataset.DateOfEdition = DateTime.UtcNow.ToString();
                dataset.DatasetContent = _autoMapper.Map<DatasetContent>(datasetToEdit.DatasetContent);

                int i = 0;
                foreach (var rowToDelete in datasetToEdit.DatasetContent.RowsToDelete)
                {
                    if (rowToDelete == false)
                    {
                        continue;
                    }
                    dataset.DatasetContent.NumberColumns.ToList().ForEach(z => z.AttributeValue.RemoveAt(i));
                    dataset.DatasetContent.StringColumns.ToList().ForEach(z => z.AttributeValue.RemoveAt(i));

                    i++;
                }

                var numberAttributePositionToDelete = datasetToEdit.DatasetContent.NumberColumns.Where(z => z.ColumnToDelete).Select(z => z.PositionInDataset).ToList();
                var numberAttributeToDelete = dataset.DatasetContent.NumberColumns.Where(z => numberAttributePositionToDelete.Contains(z.PositionInDataset)).ToList();

                foreach (var numberAttribute in numberAttributeToDelete)
                {
                    dataset.DatasetContent.NumberColumns.Remove(numberAttribute);
                }

                var stringAttributePositionToDelete = datasetToEdit.DatasetContent.StringColumns.Where(z => z.ColumnToDelete).Select(z => z.PositionInDataset).ToList();
                var stringAttributeToDelete = dataset.DatasetContent.StringColumns.Where(z => stringAttributePositionToDelete.Contains(z.PositionInDataset)).ToList();

                foreach (var stringAttribute in stringAttributeToDelete)
                {
                    dataset.DatasetContent.StringColumns.Remove(stringAttribute);
                }

                dataset.DatasetStatistics.NumberOfColumns = dataset.DatasetContent.NumberColumns.Count + dataset.DatasetContent.StringColumns.Count;
                dataset.DatasetStatistics.NumberOfRows = dataset.DatasetContent.NumberColumns.Count != 0
                                ? dataset.DatasetContent.NumberColumns.FirstOrDefault().AttributeValue.Count
                                : dataset.DatasetContent.StringColumns.FirstOrDefault().AttributeValue.Count;
                dataset.DatasetStatistics.NumberOfMissingValues = dataset.DatasetContent.StringColumns.Select(z => z.AttributeValue.Where(s => String.IsNullOrWhiteSpace(s))).Count();

                int globalColumnPosition = 0;
                int numberColumnsAmount = dataset.DatasetContent.NumberColumns.Count;
                int stringColumnsAmount = dataset.DatasetContent.StringColumns.Count;
                int n = 0;
                int s = 0;

                if (numberColumnsAmount == 0)
                {
                    dataset.DatasetContent.StringColumns.ToList().ForEach(z => z.PositionInDataset = globalColumnPosition++);
                }
                else if (stringColumnsAmount == 0)
                {
                    dataset.DatasetContent.NumberColumns.ToList().ForEach(z => z.PositionInDataset = globalColumnPosition++);
                }
                else
                {
                    for (globalColumnPosition = 0; globalColumnPosition < numberColumnsAmount + stringColumnsAmount; globalColumnPosition++)
                    {
                        if (n == numberColumnsAmount)
                        {
                            dataset.DatasetContent.StringColumns[n].PositionInDataset = globalColumnPosition;
                            s++;
                        }
                        else if (s == stringColumnsAmount)
                        {
                            dataset.DatasetContent.NumberColumns[n].PositionInDataset = globalColumnPosition;
                            n++;
                        }

                        if (dataset.DatasetContent.NumberColumns[n].PositionInDataset < dataset.DatasetContent.StringColumns[s].PositionInDataset)
                        {
                            dataset.DatasetContent.NumberColumns[n].PositionInDataset = globalColumnPosition;

                            if (n + 1 < numberColumnsAmount)
                            {
                                n++;
                            }
                        }
                        else
                        {
                            dataset.DatasetContent.StringColumns[n].PositionInDataset = globalColumnPosition;

                            if (s + 1 < stringColumnsAmount)
                            {
                                s++;
                            }
                        }
                    }
                }

                _context.datasetRepository.UpdateDataset(dataset);

                return RedirectToAction("DatasetDetails", "Dataset", new { datasetIdentificator = datasetToEdit.DatasetIdentificator });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid data has been entered.");
                return View(datasetToEdit);
            }
        }
    }
}