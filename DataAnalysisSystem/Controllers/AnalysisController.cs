using Akka.Actor;
using AutoMapper;
using DataAnalysisSystem.DataAnalysisCommands;
using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.AnalysisDTO;
using DataAnalysisSystem.DTO.AnalysisParametersDTO;
using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using DataAnalysisSystem.DTO.AnalysisResultsDTO;
using DataAnalysisSystem.DTO.Dictionaries;
using DataAnalysisSystem.Extensions;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;
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

            //var test = _context.analysisRepository.GetAnalysisById("606b0a1c3305881f84bd122a");

            //Analysis analysisTest = new Analysis();
            //analysisTest.AnalysisIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString();
            //analysisTest.DatasetIdentificator = "TESTID";
            //analysisTest.DateOfCreation = DateTime.Now.ToString();
            //analysisTest.IsShared = false;
            //analysisTest.AccessKey = "000";

            //analysisTest.AnalysisResults = new AnalysisResults();
            ////analysisTest.AnalysisResults.AnalysisResultsIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString();
            //analysisTest.AnalysisResults.HistogramResult = new HistogramResult();
            //analysisTest.AnalysisResults.BasicStatisticsResult = new BasicStatisticsResult();

            //analysisTest.AnalysisParameters = new AnalysisParameters();
            ////analysisTest.AnalysisParameters.AnalysisParametersIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString();
            //analysisTest.AnalysisParameters.ApproximationParameters = new ApproximationParameters();
            //analysisTest.AnalysisParameters.KMeansClusteringParameters = new KMeansClusteringParameters();

            //_context.analysisRepository.AddAnalysis(analysisTest);

            PerformNewAnalysisViewModel vm = new PerformNewAnalysisViewModel();
            vm.DatasetIdentificator = datasetIdentificator;

            AddAnalysisParametersViewModel paramsa = new AddAnalysisParametersViewModel()
            {
                ApproximationParameters = new AddApproximationParametersViewModel(),
                BasicStatisticsParameters = new AddBasicStatisticsParametersViewModel(),
                DeriverativeParameters = new AddDeriverativeParametersViewModel(),
                HistogramParameters = new AddHistogramParametersViewModel(),
                RegressionParameters = new AddRegressionParametersViewModel(),
                KMeansClusteringParameters = new AddKMeansClusteringParametersViewModel()
            };

            vm.AnalysisParameters = paramsa;

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PerformNewAnalysis(PerformNewAnalysisViewModel newAnalysis)
        {
            ModelState.Clear();

            var currentUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            Dataset dataset = _context.datasetRepository.GetDatasetById(newAnalysis.DatasetIdentificator);

            AnalysisParameters parameters = _autoMapper.Map<AnalysisParameters>(newAnalysis.AnalysisParameters);
            parameters = _analysisHub.SelectAnalysisParameters(newAnalysis.SelectedAnalysisMethods, parameters);

            AddAnalysisParametersViewModel modelToValidate = _autoMapper.Map<AddAnalysisParametersViewModel>(parameters);
            //modelToValidate.BasicStatisticsParameters = new AddBasicStatisticsParametersViewModel()
            //{
            //    LastName = "a"
            //};

            if (TryValidateModel(modelToValidate))
            {
                _analysisService.InitService(dataset.DatasetContent, parameters, _akkaSystem);
                List<AAnalysisCommand> commands = _analysisHub.SelectCommandsToPerform(newAnalysis.SelectedAnalysisMethods, _analysisService);

                _analysisHub.ExecuteCommandsToPerformAnalysis(commands);
                AnalysisResults analysisResults = _analysisHub.GetAnalysisResultsFromExecutedCommands(commands);

                Analysis performedAnalysis = new Analysis()
                {
                    AnalysisIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString(),
                    AnalysisIndexer = _codeGenerator.GenerateRandomKey(4),
                    AccessKey = "000",

                    AnalysisParameters = parameters,
                    AnalysisResults = analysisResults,

                    DatasetIdentificator = dataset.DatasetIdentificator,
                    DateOfCreation = DateTime.Now.ToString(),
                    IsShared = false,
                    PerformedAnalysisTypes = newAnalysis.SelectedAnalysisMethods.ToList()
                };

                currentUser.UserAnalyses.Add(performedAnalysis.AnalysisIdentificator);

                _context.analysisRepository.AddAnalysis(performedAnalysis);
                _context.userRepository.UpdateUser(currentUser);

                return RedirectToAction("AnalysisDetails", "Analysis", new { analysisIdentificator = performedAnalysis.AnalysisIdentificator, notificationMessage = "The data analysis has been completed." });
            }

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
            var currentUser = _context.userRepository.GetUserByName(this.User.Identity.Name);
            List<Analysis> userAnalyses = _context.analysisRepository.GetAnalysesById(currentUser.UserAnalyses).ToList();
            List<Dataset> analysesRelatedDatasets = _context.datasetRepository.GetDatasetsById(userAnalyses.Select(z => z.DatasetIdentificator).ToList()).ToList();

            List<AnalysisOverallInformationViewModel> userAnalysesDTO = _autoMapper.Map<List<AnalysisOverallInformationViewModel>>(userAnalyses);

            for (int i = 0; i < analysesRelatedDatasets.Count; i++)
            {
                userAnalysesDTO[i] = _autoMapper.Map<Dataset, AnalysisOverallInformationViewModel>(analysesRelatedDatasets[i], userAnalysesDTO[i]);

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
            _context.userRepository.RemoveAnalysisFromOwner(loggedUser.Id.ToString(), analysisIdentificator);
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
            List<Dataset> analysesRelatedDatasets = _context.datasetRepository.GetDatasetsById(userAnalyses.Select(z => z.DatasetIdentificator).ToList()).ToList();

            ShareAnalysisActionViewModel sharedAnalysisInfo = new ShareAnalysisActionViewModel();

            int i = 0;
            foreach (var analysis in userAnalyses)
            {
                if (analysis.IsShared)
                {
                    SharedAnalysisByOwnerViewModel sharedAnalysis = _autoMapper.Map<SharedAnalysisByOwnerViewModel>(analysis);
                    sharedAnalysis = _autoMapper.Map<Dataset, SharedAnalysisByOwnerViewModel>(analysesRelatedDatasets[i], sharedAnalysis);

                    string urlToAction = Url.GenerateLinkToSharedAnalysis(sharedAnalysis.AccessKey, Request.Scheme);
                    sharedAnalysis.UrlToAction = urlToAction;

                    sharedAnalysisInfo.SharedAnalyses.Add(sharedAnalysis);
                }
                else
                {
                    NotSharedAnalysisViewModel notSharedAnalysis = _autoMapper.Map<NotSharedAnalysisViewModel>(analysis);
                    notSharedAnalysis = _autoMapper.Map<Dataset, NotSharedAnalysisViewModel>(analysesRelatedDatasets[i], notSharedAnalysis);

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
            List<Dataset> analysesRelatedDatasets = _context.datasetRepository.GetDatasetsById(analysesShared.Select(z => z.DatasetIdentificator).ToList()).ToList();

            SharedAnalysesBrowserViewModel analysesSharedVm = new SharedAnalysesBrowserViewModel();
            analysesSharedVm.SharedAnalyses = _autoMapper.Map<List<SharedAnalysisByCollabViewModel>>(analysesShared).ToList();

            for (int i = 0; i < analysesSharedVm.SharedAnalyses.Count; i++)
            {
                analysesSharedVm.SharedAnalyses[i] = _autoMapper.Map<Dataset, SharedAnalysisByCollabViewModel>(analysesRelatedDatasets[i], analysesSharedVm.SharedAnalyses[i]);
                analysesSharedVm.SharedAnalyses[i].OwnerName = GetAnalysisOwnerName(analysesSharedVm.SharedAnalyses[i].AnalysisIdentificator);
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
        public IActionResult AnalysisDetails(string analysisIdentificator)
        {
            Analysis analysis = _context.analysisRepository.GetAnalysisById(analysisIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (loggedUser.SharedAnalysesToUser.Contains(analysisIdentificator))
            {
                return RedirectToAction("SharedAnalysisDetails", "Analysis", new { analysisIdentificator = analysisIdentificator });
            }
            else if (!loggedUser.UserAnalyses.Contains(analysisIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            //TO DO:
            AnalysisDetailsViewModel analysisDetails = _autoMapper.Map<AnalysisDetailsViewModel>(analysis);
            analysisDetails.AnalysisResults = _autoMapper.Map<AnalysisResultsDetailsViewModel>(analysis.AnalysisResults);
            analysisDetails.AnalysisParameters = _autoMapper.Map<AnalysisParametersDetailsViewModel>(analysis.AnalysisParameters);

            return View(analysisDetails);
        }

        [Authorize]
        [HttpGet]
        public IActionResult SharedAnalysisDetails(string analysisIdentificator)
        {
            Analysis analysis = _context.analysisRepository.GetAnalysisById(analysisIdentificator);
            var loggedUser = _context.userRepository.GetUserByName(this.User.Identity.Name);

            if (loggedUser.UserAnalyses.Contains(analysisIdentificator))
            {
                return RedirectToAction("AnalysisDetails", "Analysis", new { analysisIdentificator = analysisIdentificator });
            }
            else if (!loggedUser.SharedAnalysesToUser.Contains(analysisIdentificator))
            {
                return RedirectToAction("MainAction", "UserSystemInteraction");
            }

            //TO DO:
            SharedAnalysisDetailsViewModel analysisDetails = _autoMapper.Map<SharedAnalysisDetailsViewModel>(analysis);
            analysisDetails.AnalysisResults = _autoMapper.Map<AnalysisResultsDetailsViewModel>(analysis.AnalysisResults);
            analysisDetails.AnalysisParameters = _autoMapper.Map<AnalysisParametersDetailsViewModel>(analysis.AnalysisParameters);

            if (loggedUser.SharedDatasetsToUser.Contains(analysis.DatasetIdentificator))
            {
                analysisDetails.UserHasAccessToDataset = true;
            }

            return View(analysisDetails);
        }
    }
}
