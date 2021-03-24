using AutoMapper;
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
using System.Threading.Tasks;

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
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteAnalysis()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserSharedAnalysis()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult SharedAnalysisBrowser()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SharedAnalysisBrowser(int a)
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult SharedAnalysisDetails()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditAnalysis()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditAnalysis(int a)
        {
            return View();
        }
    }
}
