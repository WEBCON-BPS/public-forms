using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class BpsDataController : Controller
    {
        private readonly IBpsQueryService bpsFormService;
        private readonly ILogger<BpsDataController> logger;
        private readonly IStringLocalizer<BpsDataController> localizer;

        public BpsDataController(IBpsQueryService bpsFormService, ILogger<BpsDataController> logger, IStringLocalizer<BpsDataController> localizer)
        {
            this.bpsFormService = bpsFormService;
            this.logger = logger;
            this.localizer = localizer;
        }
        [HttpPost]
        public async Task<JsonResult> GetToken()
        {
            try
            {
                var result = await bpsFormService.Authenticate();
                return Json(new { token = result });
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
                return new JsonResult(localizer?[ex.Message] ?? ex.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetApplications()
        {
            return await GetObjects(() => { return bpsFormService.GetApplicationsAsync(); });
        }
        [HttpPost]
        public async Task<JsonResult> GetProcesses(Guid parentGuid)
        {
            return await GetObjects(() => { return bpsFormService.GetProcessesAsync(parentGuid); });
        }
        [HttpPost]
        public async Task<JsonResult> GetWorkflows(Guid parentGuid)
        {
            return await GetObjects(() => { return bpsFormService.GetWorkflowsAsync(parentGuid); });
        }
        [HttpPost]
        public async Task<JsonResult> GetForms(Guid parentGuid)
        {
            return await GetObjects(() => { return bpsFormService.GetAssociatedFormTypesAsync(parentGuid); });
        }
        [HttpPost]
        public async Task<JsonResult> GetStartStep(Guid parentGuid)
        {
            try
            {
                var result = await bpsFormService.GetStartStepAsync(parentGuid);
                return Json(new KeyValuePair<Guid, string>(result.Guid, result.Name));
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
                return Json(localizer?[ex.Message] ?? ex.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetStepPaths(Guid parentGuid)
        {
            return await GetObjects(() => { return bpsFormService.GetStepPathsAsync(parentGuid); });
        }
        [HttpPost]
        public async Task<JsonResult> GetBusinessEntities()
        {
            return await GetObjects(() => { return bpsFormService.GetBusinessEntities(); });
        }
        private async Task<JsonResult> GetObjects<T>(Func<Task<IEnumerable<T>>> getObjectsAction) where T : BaseObject
        {
            try
            {
                var result = await getObjectsAction?.Invoke();
                return Json(result.OrderBy(x => x.Name).Select(x => new KeyValuePair<Guid, string>(x.Guid, x.Name)).ToList());
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
                return new JsonResult(localizer?[ex.Message] ?? ex.Message);
            }
        }
    }
}
