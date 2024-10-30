using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation.Configuration.Model;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class BpsCheckConnectionController : Controller
    {
        private readonly IBpsCheckConnectionService checkConnectionService;
        private readonly IStringLocalizer<BpsCheckConnectionController> localizer;
        private readonly IReadOnlyConfiguration _config;

        public BpsCheckConnectionController(IBpsCheckConnectionService checkConnectionService, IStringLocalizer<BpsCheckConnectionController> localizer,
            IReadOnlyConfiguration config)
        {
            this.checkConnectionService = checkConnectionService;
            this.localizer = localizer;
            _config = config;
        }
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> CheckConnection()
        {
            try
            {
                if (string.IsNullOrEmpty(_config.ApiSettings.Url) || string.IsNullOrEmpty(_config.ApiSettings.ClientId) || string.IsNullOrEmpty(_config.ApiSettings.ClientSecret))
                    return new JsonResult(new CheckConnectionResultViewModel { IsConnected = false, ResultMessage = localizer?["Data for login not provided"] ?? "Data for login not provided" });

                CheckConnectionResult response = 
                    await checkConnectionService.CheckConnection(_config.ApiSettings.ClientId, _config.ApiSettings.ClientSecret, _config.ApiSettings.Url);
                return new JsonResult(new CheckConnectionResultViewModel { IsConnected = response.IsConnected, ResultMessage = localizer?[response.ResultMessage]?.Value??response.ResultMessage });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CheckConnectionResultViewModel { IsConnected = false, ResultMessage = ex.Message });
            }

        }
    }
}
