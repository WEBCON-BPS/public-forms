using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class BpsCheckConnectionController : Controller
    {
        private readonly IBpsCheckConnectionService checkConnectionService;
        private readonly IStringLocalizer<BpsCheckConnectionController> localizer;

        public BpsCheckConnectionController(IBpsCheckConnectionService checkConnectionService, IStringLocalizer<BpsCheckConnectionController> localizer)
        {
            this.checkConnectionService = checkConnectionService;
            this.localizer = localizer;
        }
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> CheckConnection(string apiUrl, string apiClientId, string apiClientSecret)
        {
            try
            {
                if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(apiClientId) || string.IsNullOrEmpty(apiClientSecret))
                    return new JsonResult(new CheckConnectionResultViewModel { IsConnected = false, ResultMessage = localizer?["Data for login not provided"] ?? "Data for login not provided" });

                CheckConnectionResult response = await checkConnectionService.CheckConnection(apiClientId, apiClientSecret, apiUrl);
                return new JsonResult(new CheckConnectionResultViewModel { IsConnected = response.IsConnected, ResultMessage = localizer?[response.ResultMessage]?.Value??response.ResultMessage });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CheckConnectionResultViewModel { IsConnected = false, ResultMessage = ex.Message });
            }

        }
    }
}
