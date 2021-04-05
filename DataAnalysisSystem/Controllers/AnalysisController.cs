using AutoMapper;
using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.AnalysisDTO;
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

        private readonly ISerializerStrategy _serializerStrategy;

        private readonly CustomSerializer _customSerializer;

        private readonly IMapper _autoMapper;

        private readonly RepositoryContext _context;

        public AnalysisController(
                                  RepositoryContext context,
                                  CustomSerializer customSerializer,
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

            this._customSerializer = customSerializer;

            this._autoMapper = autoMapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult PerformNewAnalysis()
        {
            var test = _context.analysisRepository.GetAnalysisById("606b0a1c3305881f84bd122a");

            Analysis analysisTest = new Analysis();
            analysisTest.AnalysisIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString();
            analysisTest.DatasetIdentificator = "TESTID";
            analysisTest.DateOfCreation = DateTime.Now.ToString();
            analysisTest.IsShared = false;
            analysisTest.AccessKey = "000";

            analysisTest.AnalysisResults = new AnalysisResults();
            //analysisTest.AnalysisResults.AnalysisResultsIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString();
            analysisTest.AnalysisResults.HistogramResult = new HistogramResult();
            analysisTest.AnalysisResults.BasicStatisticsResult = new BasicStatisticsResult();

            analysisTest.AnalysisParameters = new AnalysisParameters();
            //analysisTest.AnalysisParameters.AnalysisParametersIdentificator = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsString();
            analysisTest.AnalysisParameters.ApproximationParameters = new ApproximationParameters();
            analysisTest.AnalysisParameters.KMeansClusteringParameters = new KMeansClusteringParameters();

            _context.analysisRepository.AddAnalysis(analysisTest);

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult PerformNewAnalysis(int a)
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
            }

            return View(userAnalysesDTO);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteAnalysis()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserSharedAnalysis(string notificationMessage = null)
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
            analysisToShare.AccessKey = _codeGenerator.GenerateAccessKey(8);

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
        public IActionResult SharedDatasetsBrowserPost(string newSharedAnalysisAccessKey)
        {
            return RedirectToAction("GainAccessToSharedAnalysis", new { analysisAccessKey = newSharedAnalysisAccessKey });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GainAccessToSharedAnalysis(string analysisAccessKey)
        {
            //TODO
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult SharedAnalysisDetails()
        {
            return View();
        }
    }
}
