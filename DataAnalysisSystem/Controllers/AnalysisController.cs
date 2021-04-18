using Akka.Actor;
using AutoMapper;
using DataAnalysisSystem.DataAnalysisCommands;
using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.AnalysisDTO;
using DataAnalysisSystem.DTO.AnalysisParametersDTO;
using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using DataAnalysisSystem.DTO.AnalysisResultsDTO;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.DTO.Dictionaries;
using DataAnalysisSystem.Extensions;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly ICodeGenerator _codeGenerator;
        private readonly IEmailProvider _emailProvider;
        private readonly IRegexComparatorChainFacade _regexComparator;
        private readonly IMimeTypeGuesser _mimeTypeGuesser;
        private readonly IFileHelper _fileHelper;

        private readonly CustomSerializer _customSerializer;

        private readonly IMapper _autoMapper;

        private readonly RepositoryContext _context;

        private readonly IDataAnalysisHub _analysisHub;
        private readonly IDataAnalysisService _analysisService;

        private readonly ActorSystem _akkaSystem;

        public AnalysisController(RepositoryContext context,
                                  CustomSerializer customSerializer,
                                  ICodeGenerator codeGenerator,
                                  IEmailProvider emailProvider,
                                  IMapper autoMapper,
                                  IRegexComparatorChainFacade regexComparator,
                                  IMimeTypeGuesser mimeTypeGuesser,
                                  IFileHelper fileHelper,
                                  IDataAnalysisHub analysisHub,
                                  IDataAnalysisService analysisService,
                                  ActorSystem akkaSystem)
        {
            this._context = context;

            this._codeGenerator = codeGenerator;
            this._emailProvider = emailProvider;
            this._regexComparator = regexComparator;
            this._mimeTypeGuesser = mimeTypeGuesser;
            this._fileHelper = fileHelper;

            this._customSerializer = customSerializer;

            this._autoMapper = autoMapper;

            this._analysisHub = analysisHub;
            this._analysisService = analysisService;

            this._akkaSystem = akkaSystem;
        }

        [Authorize]
        [HttpGet]
        public IActionResult PerformNewAnalysis(string datasetIdentificator, string notificationMessage)
        {
            ViewData["Message"] = notificationMessage;

            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            Dataset relatedDataset = _context.datasetRepository.GetDatasetById(datasetIdentificator);
            DatasetContentViewModel relatedDatasetContent = _autoMapper.Map<DatasetContentViewModel>(relatedDataset.DatasetContent);

            if (relatedDataset == null || (!loggedUser.UserDatasets.Contains(relatedDataset.DatasetIdentificator) && !loggedUser.SharedDatasetsToUser.Contains(relatedDataset.DatasetIdentificator)))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            PerformNewAnalysisViewModel performAnalysisViewModel = _autoMapper.Map<PerformNewAnalysisViewModel>(relatedDataset);
           
            performAnalysisViewModel.AnalysisParameters = new AddAnalysisParametersViewModel();
            performAnalysisViewModel.AnalysisParameters.HistogramParameters = new AddHistogramParametersViewModel(relatedDatasetContent);
            performAnalysisViewModel.AnalysisParameters.BasicStatisticsParameters = new AddBasicStatisticsParametersViewModel(relatedDatasetContent);
            performAnalysisViewModel.AnalysisParameters.KMeansClusteringParameters = new AddKMeansClusteringParametersViewModel(relatedDatasetContent);
            performAnalysisViewModel.AnalysisParameters.ApproximationParameters = new AddApproximationParametersViewModel(relatedDatasetContent);
            performAnalysisViewModel.AnalysisParameters.DeriverativeParameters = new AddDeriverativeParametersViewModel(relatedDatasetContent);
            performAnalysisViewModel.AnalysisParameters.RegressionParameters = new AddRegressionParametersViewModel(relatedDatasetContent);

            return View(performAnalysisViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PerformNewAnalysis(PerformNewAnalysisViewModel newAnalysis)
        {
            ModelState.Clear();

            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            Dataset relatedDataset = _context.datasetRepository.GetDatasetById(newAnalysis.DatasetIdentificator);

            if (relatedDataset == null || (!loggedUser.UserDatasets.Contains(relatedDataset.DatasetIdentificator) && !loggedUser.SharedDatasetsToUser.Contains(relatedDataset.DatasetIdentificator)))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            AnalysisParameters parameters = _autoMapper.Map<AnalysisParameters>(newAnalysis.AnalysisParameters);
            parameters = _analysisHub.SelectAnalysisParameters(newAnalysis.SelectedAnalysisMethods, parameters);

            AddAnalysisParametersViewModel modelToValidate = _autoMapper.Map<AddAnalysisParametersViewModel>(parameters);

            if (TryValidateModel(modelToValidate) && TryValidateModel(newAnalysis.AnalysisName))
            {
                _analysisService.InitService(relatedDataset.DatasetContent, parameters, _akkaSystem);
                List<AAnalysisCommand> commands = _analysisHub.SelectCommandsToPerform(newAnalysis.SelectedAnalysisMethods, _analysisService);

                _analysisHub.ExecuteCommandsToPerformAnalysis(commands);
                AnalysisResults analysisResults = _analysisHub.GetAnalysisResultsFromExecutedCommands(commands);

                Analysis performedAnalysis = new Analysis()
                {
                    AnalysisIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString(),
                    AnalysisName = newAnalysis.AnalysisName,
                    AnalysisIndexer = _codeGenerator.GenerateRandomKey(4),
                    AccessKey = "000",

                    AnalysisParameters = parameters,
                    AnalysisResults = analysisResults,

                    DatasetIdentificator = relatedDataset.DatasetIdentificator,
                    DateOfCreation = DateTime.Now.ToString(),
                    IsShared = false,
                    PerformedAnalysisMethods = newAnalysis.SelectedAnalysisMethods.ToList()
                };

                loggedUser.UserAnalyses.Add(performedAnalysis.AnalysisIdentificator);

                _context.analysisRepository.AddAnalysis(performedAnalysis);
                _context.userRepository.UpdateUser(loggedUser);

                return RedirectToAction("AnalysisDetails", "Analysis", new { analysisIdentificator = performedAnalysis.AnalysisIdentificator, notificationMessage = "The data analysis has been completed." });
            }

            ModelState.AddModelError(string.Empty, "The parameters entered contain invalid settings.");
            return View(newAnalysis);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AvailableAnalysisMethodsOverview()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserAnalyses()
        {
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            List<Analysis> userAnalyses = _context.analysisRepository.GetAnalysesById(loggedUser.UserAnalyses).ToList();

            List<AnalysisOverallInformationViewModel> userAnalysesDTO = _autoMapper.Map<List<AnalysisOverallInformationViewModel>>(userAnalyses);

            for (int i = 0; i < userAnalyses.Count; i++)
            {
                Dataset relatedDataset = _context.datasetRepository.GetDatasetById(userAnalyses[i].DatasetIdentificator);
                userAnalysesDTO[i] = _autoMapper.Map<Dataset, AnalysisOverallInformationViewModel>(relatedDataset, userAnalysesDTO[i]);

                for (int j = 0; j < userAnalysesDTO[i].PerformedAnalysisMethods.Count; j++)
                {
                    userAnalysesDTO[i].PerformedAnalysisMethods[j] = AnalysisTypeDictionary.AnalysisType.GetValueOrDefault(userAnalysesDTO[i].PerformedAnalysisMethods[j]);
                }
            }

            return View(userAnalysesDTO);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteAnalysis(string analysisIdentificator)
        {
            Analysis analysis = _context.analysisRepository.GetAnalysisById(analysisIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (analysis == null || !loggedUser.UserAnalyses.Contains(analysisIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            _context.analysisRepository.DeleteAnalysis(analysisIdentificator);
            _context.userRepository.RemoveAnalysisFromOwner(loggedUser.Id, analysisIdentificator);
            _context.userRepository.RemoveSharedAnalysisFromUsers(analysisIdentificator);

            return RedirectToAction("MainAction", "UserSystemInteraction", new { notificationMessage = "The analysis was successfully removed from the system." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserSharedAnalyses(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            List<Analysis> userAnalyses = _context.analysisRepository.GetAnalysesById(loggedUser.UserAnalyses).ToList();

            ShareAnalysisActionViewModel sharedAnalysisInfo = new ShareAnalysisActionViewModel();

            int i = 0;
            foreach (var analysis in userAnalyses)
            {
                Dataset relatedDataset = _context.datasetRepository.GetDatasetById(analysis.DatasetIdentificator);

                for (int j = 0; j < analysis.PerformedAnalysisMethods.Count; j++)
                {
                    analysis.PerformedAnalysisMethods[j] = AnalysisTypeDictionary.AnalysisType.GetValueOrDefault(analysis.PerformedAnalysisMethods[j]);
                }

                if (analysis.IsShared)
                {
                    SharedAnalysisByOwnerViewModel sharedAnalysis = _autoMapper.Map<SharedAnalysisByOwnerViewModel>(analysis);
                    sharedAnalysis = _autoMapper.Map<Dataset, SharedAnalysisByOwnerViewModel>(relatedDataset, sharedAnalysis);

                    string urlToAction = Url.GenerateLinkToSharedAnalysis(sharedAnalysis.AccessKey, Request.Scheme);
                    sharedAnalysis.UrlToAction = urlToAction;

                    sharedAnalysisInfo.SharedAnalyses.Add(sharedAnalysis);
                }
                else
                {
                    NotSharedAnalysisViewModel notSharedAnalysis = _autoMapper.Map<NotSharedAnalysisViewModel>(analysis);
                    notSharedAnalysis = _autoMapper.Map<Dataset, NotSharedAnalysisViewModel>(relatedDataset, notSharedAnalysis);

                    sharedAnalysisInfo.NotSharedAnalyses.Add(notSharedAnalysis);
                }
                i++;
            }

            return View(sharedAnalysisInfo);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ShareAnalysis(string analysisIdentificator)
        {
            Analysis analysisToShare = _context.analysisRepository.GetAnalysisById(analysisIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (analysisToShare == null || !loggedUser.UserAnalyses.Contains(analysisIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            analysisToShare.IsShared = true;
            analysisToShare.AccessKey = _codeGenerator.GenerateRandomKey(8);

            _context.analysisRepository.UpdateAnalysis(analysisToShare);

            return RedirectToAction("UserSharedAnalysis", "Analysis", new { notificationMessage = "A analysis has been made available." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult StopSharingAnalysis(string analysisIdentificator)
        {
            Analysis analysis = _context.analysisRepository.GetAnalysisById(analysisIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (analysis == null || !loggedUser.UserAnalyses.Contains(analysisIdentificator) || !analysis.IsShared)
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            _context.userRepository.RemoveSharedAnalysisFromUsers(analysisIdentificator);

            analysis.IsShared = false;
            analysis.AccessKey = "000";
            _context.analysisRepository.UpdateAnalysis(analysis);

            return RedirectToAction("UserSharedAnalysis", "Analysis", new { notificationMessage = "The selected analysis is no longer shared with other system users." });
        }

        public string GetAnalysisOwnerName(string analysisIdentificator)
        {
            IdentityProviderUser analysisOwner = _context.userRepository.GetAnalysisOwnerByAnalysisId(analysisIdentificator);

            if (analysisOwner == null)
            {
                return "not found";
            }

            return analysisOwner.FirstName + " " + analysisOwner.LastName;
        }

        public SharedAnalysesBrowserViewModel GetAnalysesSharedToLoggedUser()
        {
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            List<Analysis> analysesShared = _context.analysisRepository.GetAnalysesById(loggedUser.SharedAnalysesToUser).ToList();

            SharedAnalysesBrowserViewModel analysesSharedVm = new SharedAnalysesBrowserViewModel();
            analysesSharedVm.SharedAnalyses = _autoMapper.Map<List<SharedAnalysisByCollabViewModel>>(analysesShared).ToList();

            for (int i = 0; i < analysesSharedVm.SharedAnalyses.Count; i++)
            {
                Dataset relatedDataset = _context.datasetRepository.GetDatasetById(analysesShared[i].DatasetIdentificator);

                analysesSharedVm.SharedAnalyses[i] = _autoMapper.Map<Dataset, SharedAnalysisByCollabViewModel>(relatedDataset, analysesSharedVm.SharedAnalyses[i]);
                analysesSharedVm.SharedAnalyses[i].OwnerName = GetAnalysisOwnerName(analysesSharedVm.SharedAnalyses[i].AnalysisIdentificator);

                for (int j = 0; j < analysesSharedVm.SharedAnalyses[i].PerformedAnalysisMethods.Count; j++)
                {
                    analysesSharedVm.SharedAnalyses[i].PerformedAnalysisMethods[j] = AnalysisTypeDictionary.AnalysisType.GetValueOrDefault(analysesSharedVm.SharedAnalyses[i].PerformedAnalysisMethods[j]);
                }
            }

            return analysesSharedVm;
        }

        [Authorize]
        [HttpGet]
        public IActionResult SharedAnalysesBrowser(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            SharedAnalysesBrowserViewModel analysisBrowser = GetAnalysesSharedToLoggedUser();

            return View(analysisBrowser);
        }

        [Authorize]
        [HttpPost]
        [ActionName("SharedAnalysesBrowser")]
        public IActionResult SharedAnalysesBrowserPost(string newSharedAnalysisAccessKey)
        {
            return RedirectToAction("GainAccessToSharedAnalysis", new { analysisAccessKey = newSharedAnalysisAccessKey });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GainAccessToSharedAnalysis(string analysisAccessKey)
        {
            Analysis analysisShared = _context.analysisRepository.GetAnalysisByAccessKey(analysisAccessKey);

            if (analysisShared != null)
            {
                var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

                if (loggedUser.UserAnalyses.Contains(analysisShared.AnalysisIdentificator))
                {
                    ModelState.AddModelError(string.Empty, "You are the owner of this analysis.");
                    SharedAnalysesBrowserViewModel analysesBrowser = GetAnalysesSharedToLoggedUser();

                    return View("SharedAnalysesBrowser", analysesBrowser);
                }
                if (loggedUser.SharedAnalysesToUser.Contains(analysisShared.AnalysisIdentificator))
                {
                    ModelState.AddModelError(string.Empty, "You already have access to this analysis.");
                    SharedAnalysesBrowserViewModel analysesBrowser = GetAnalysesSharedToLoggedUser();

                    return View("SharedAnalysesBrowser", analysesBrowser);
                }

                loggedUser.SharedAnalysesToUser.Add(analysisShared.AnalysisIdentificator);
                _context.userRepository.UpdateUser(loggedUser);

                return RedirectToAction("SharedAnalysesBrowser", "Analysis", new { notificationMessage = "Access was gained to a shared analysis." });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid access key.");
                SharedAnalysesBrowserViewModel analysesBrowser = GetAnalysesSharedToLoggedUser();

                return View("SharedAnalysesBrowser", analysesBrowser);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult AnalysisDetails(string analysisIdentificator, string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            Analysis analysis = _context.analysisRepository.GetAnalysisById(analysisIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (!loggedUser.UserAnalyses.Contains(analysisIdentificator) && !loggedUser.SharedAnalysesToUser.Contains(analysisIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            AnalysisDetailsViewModel analysisDetails = _autoMapper.Map<AnalysisDetailsViewModel>(analysis);
            analysisDetails.AnalysisResults = _autoMapper.Map<AnalysisResultsDetailsViewModel>(analysis.AnalysisResults);
            analysisDetails.AnalysisParameters = _autoMapper.Map<AnalysisParametersDetailsViewModel>(analysis.AnalysisParameters);

            if (loggedUser.SharedDatasetsToUser.Contains(analysis.DatasetIdentificator) || loggedUser.UserDatasets.Contains(analysis.DatasetIdentificator))
            {
                analysisDetails.UserHasAccessToDataset = true;
            }
            if (loggedUser.UserAnalyses.Contains(analysisIdentificator))
            {
                analysisDetails.UserIsOwnerOfAnalysis = true;
            }

            return View(analysisDetails);
        }
    }
}
