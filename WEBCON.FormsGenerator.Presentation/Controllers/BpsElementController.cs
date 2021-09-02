using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class BpsElementController : Controller
    {
        private readonly IBpsStartElementService commandService;
        private readonly ILogger<BpsElementController> logger;

        public BpsElementController(IBpsStartElementService commandService, ILogger<BpsElementController> logger)
        {
            this.commandService = commandService;
            this.logger = logger;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Start(IFormCollection collection, string culture)
        {
            string formValue = "";
            try
            {
                if (!string.IsNullOrEmpty(culture))
                {
                    CultureInfo cultureInfo = new CultureInfo(culture);
                    Thread.CurrentThread.CurrentCulture = cultureInfo;
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                }
                if (collection == null || !collection.Any())
                    throw new ApplicationArgumentException("Could not get form body");
                formValue = collection.FirstOrDefault(x => x.Key.Equals("formGuid")).Value;
                if (formValue == null)
                    throw new ApplicationArgumentException("Form guid not provided");

                if (!(collection is FormCollection formCollection)) throw new Exception("Could not get form fields");

                List<KeyValuePair<Guid, object>> formFields = new List<KeyValuePair<Guid, object>>();
                foreach (var formCollectionField in formCollection.Where(x => Guid.TryParse(x.Key, out _)))
                {
                    formFields.Add(new KeyValuePair<Guid, object>(new Guid(formCollectionField.Key), formCollectionField.Value));
                }
                Guid formGuid = new Guid(formValue);
                StartElementResult result = await commandService.Start(formFields, formGuid);
                return new JsonResult(new StartElementViewModel
                {
                    ElementId = result.Id,
                    ElementNumber = result.Number,
                    StartingElementFormGuid = formValue
                });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                logger?.LogError(ex, ex.Message);
                return new JsonResult(ex.Message);
            }
        }
    }
}
