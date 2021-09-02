using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class FormsController : Controller
    {
        private readonly IFormQueryService formQueryService;
        private readonly ILogger<FormsController> logger;
        private readonly IStringLocalizer<FormsController> localizer;

        public FormsController(IFormQueryService formQueryService, ILogger<FormsController> logger, IStringLocalizer<FormsController> localizer)
        {
            this.formQueryService = formQueryService;
            this.logger = logger;
            this.localizer = localizer;
        }
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewData["CurrentFilter"] = searchString;

                logger?.LogInformation("Forms have been loaded");

                var forms = await formQueryService.GetForms(searchString, pageNumber ?? 1, sortOrder, 20);
                if (forms == null)
                    return View();

                return View(forms);
            }
            catch(BusinessLogic.Domain.Exceptions.ApplicationException ex)
            {
                ViewData["Error"] = localizer["Could not load data."] + $"({localizer[ex.Message]})";
                logger?.LogError(ex, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = localizer["Could not load data. Contact with administrator."];
                logger?.LogError(ex, ex.Message);
                return View();
            }
        }
    }
}
