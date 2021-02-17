using AutoMapper;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.ServicesInterfaces;
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
        private readonly ICodeGenerator _codeGenerator;
        private readonly IEmailProvider _emailProvider;
        private readonly IMapper _autoMapper;

        private readonly RepositoryContext _context;

        public DatasetController(
                                 RepositoryContext context,
                                 ICodeGenerator codeGenerator,
                                 IEmailProvider emailProvider,
                                 IMapper autoMapper){

            this._context = context;

            this._codeGenerator = codeGenerator;
            this._emailProvider = emailProvider;

            this._autoMapper = autoMapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddNewDataset(string notificationMessage = null)
        {
            ViewData["notificationMessage"] = notificationMessage;
            AddNewDatasetViewModel newDataset = new AddNewDatasetViewModel();

            return View(newDataset);
        }
    }
}
