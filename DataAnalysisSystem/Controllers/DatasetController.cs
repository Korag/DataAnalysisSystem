using AutoMapper;
using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Extensions;
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

        private readonly ICodeGenerator _codeGenerator;
        private readonly ICodeQRGenerator _qrCodeGenerator;
        private readonly IEmailProvider _emailProvider;
        private readonly IRegexComparatorChainFacade _regexComparator;
        private readonly IMimeTypeGuesser _mimeTypeGuesser;
        private readonly IFileHelper _fileHelper;

        private readonly IHostingEnvironment _environment;
        private readonly ISerializerStrategy _serializerStrategy;

        private readonly CustomSerializer _customSerializer;

        private readonly IMapper _autoMapper;

        private readonly RepositoryContext _context;

        public DatasetController(
                                 RepositoryContext context,
                                 IHostingEnvironment environment,
                                 ICodeGenerator codeGenerator,
                                 ICodeQRGenerator qrCodeGenerator,
                                 IEmailProvider emailProvider,
                                 IMapper autoMapper,
                                 IRegexComparatorChainFacade regexComparator,
                                 IMimeTypeGuesser mimeTypeGuesser,
                                 IFileHelper fileHelper)
        {

            this._context = context;
            this._environment = environment;

            this._codeGenerator = codeGenerator;
            this._qrCodeGenerator = qrCodeGenerator;

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

            return RedirectToAction("SharedUserDatasets", "Dataset", new { notificationMessage = "A data set has been made available." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult StopSharingDataset(string datasetIdentificator)
        {
            Dataset dataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            _context.userRepository.RemoveSharedDatasetsFromUsers(datasetIdentificator); 

            dataset.IsShared = false;
            dataset.AccessKey = "000";
            _context.datasetRepository.UpdateDataset(dataset);

            return RedirectToAction("SharedUserDatasets", "Dataset", new { notificationMessage = "The selected dataset is no longer shared with other system users." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult ExportDataset(string datasetIdentificator)
        {
            Dataset datasetToExport = _context.datasetRepository.GetDatasetById(datasetIdentificator);

            if (datasetToExport == null)
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            ExportDatasetViewModel exportDataset = _autoMapper.Map<ExportDatasetViewModel>(datasetToExport);
            exportDataset.DatasetContent = _autoMapper.Map<DatasetContentViewModel>(datasetToExport.DatasetContent);

            return View(exportDataset);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ExportDataset(int a)
        {
            return View();
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
            datasetSharedVm.SharedDatasets = _autoMapper.Map<List<DatasetStatistics>, List<SharedDatasetByCollabViewModel>>(datasetShared.Select(z => z.DatasetStatistics).ToList());
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
        public IActionResult SharedDatasetsBrowserPost(string datasetAccessKey)
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

            return RedirectToAction("GainAccessToSharedDataset", new { datasetAccessKey = datasetAccessKey });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GainAccessToSharedDataset(string datasetAccessKey)
        {
            Dataset datasetShared = _context.datasetRepository.GetDatasetByAccessKey(datasetAccessKey);
            SharedDatasetsBrowserViewModel datasetBrowser = GetDatasetsSharedToLoggedUser();

            if (datasetShared != null)
            {
                var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

                if (loggedUser.UserDatasets.Contains(datasetShared.DatasetIdentificator))
                {
                    ModelState.AddModelError(string.Empty, "You are the owner of this data set.");

                    return View("SharedDatasetBrowser", datasetBrowser);
                }
                if (loggedUser.SharedDatasetsToUser.Contains(datasetShared.DatasetIdentificator))
                {
                    ModelState.AddModelError(string.Empty, "You already have access to this dataset.");

                    return View("SharedDatasetBrowser", datasetBrowser);
                }

                loggedUser.SharedDatasetsToUser.Add(datasetShared.DatasetIdentificator);
                _context.userRepository.UpdateUser(loggedUser);

                return View("SharedDatasetBrowser", datasetBrowser);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid access key.");

                return View("SharedDatasetBrowser", datasetBrowser);
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
                return RedirectToAction("DatasetDetails", "Dataset");
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
        public IActionResult EditDataset()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditDataset(int a)
        {
            return View();
        }
    }
}