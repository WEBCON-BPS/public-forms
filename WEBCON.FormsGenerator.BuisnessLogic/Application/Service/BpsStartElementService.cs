using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class BpsStartElementService : IBpsStartElementService
    {
        private readonly IBpsClientCommandService clientService;
        private readonly IFormUnitOfWork formUnitOfWork;
        private readonly IDataEncoding dataEncoding;
        private readonly IFormFieldFactory formFieldBuilder;
        private readonly ILogger<BpsStartElementService> logger;

        public BpsStartElementService(IBpsClientCommandService clientService, IFormUnitOfWork formUnitOfWork,
            IDataEncoding dataEncoding, IFormFieldFactory formFieldBuilder, ILogger<BpsStartElementService> logger)
        {
            this.clientService = clientService ?? throw new ApplicationArgumentException("Client service instance has not been provided");
            this.formUnitOfWork = formUnitOfWork ?? throw new ApplicationArgumentException("Data source instance has not been initialized.");
            this.dataEncoding = dataEncoding;
            this.formFieldBuilder = formFieldBuilder ?? throw new ApplicationArgumentException("Client service instance has not been provided");
            this.logger = logger;
        }

        public Task<StartElementResult> Start(IEnumerable<KeyValuePair<Guid, object>> fields, Guid formGuid)
        {
            if (fields?.Any() != true)
                throw new ApplicationArgumentException("Form fields has not been provided. Could not start element.");

            if (formGuid.Equals(Guid.Empty)) throw new ApplicationArgumentException("Form guid has not been provided.");

            Domain.Model.Form form = formUnitOfWork.Forms.FirstOrDefault(x => x.Guid == formGuid);
            if (form == null) throw new FormNotFoundException("Could not find form with specified guid.");
            
            IEnumerable<Domain.Model.BpsFormField> formFields = formUnitOfWork.BpsFormFields.GetFiltered(x => x.BPSFormType.Guid == form.BpsFormType.Guid);
            if (formFields == null) throw new NotFoundException("Form fields definition not found. Could not start element.");

            var customCredentials = CreateCustomCredentials(form.ClientId, form.ClientSecret);
            return clientService.StartElement(GetStartElementFields(fields, formFields), form.BpsStepPath?.BpsWorkflowStep?.BpsWorkflow?.Guid??Guid.Empty, 
                form.BpsFormType?.Guid?? Guid.Empty, form.BpsStepPath?.Guid?? Guid.Empty, form.BusinessEntityGuid, logger, customCredentials);
        }

        private List<FormField> GetStartElementFields(IEnumerable<KeyValuePair<Guid, object>> fields, IEnumerable<Domain.Model.BpsFormField> formFields)
        {
            List<DTO.FormField> fieldsList = new List<FormField>();
            foreach (var field in fields)
            {
                var fieldDefinition = formFields.FirstOrDefault(x => x.Guid == field.Key);
                if (fieldDefinition == null)
                    throw new NotFoundException($"Could not find form field for guid {field.Key}");
              
                fieldsList.Add(formFieldBuilder.FormFieldBuilder.GetFormField(field.Key, fieldDefinition.Name, formFieldBuilder.FormValueBuilder.GetFormFieldValue(fieldDefinition.Type,field.Value), false));
            }
            return fieldsList;
        }

        private Credentials CreateCustomCredentials(string clientId, string clientSecret)
        {
            if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret))
            {
                if (dataEncoding == null)
                    throw new ApplicationArgumentException("Could not start element because could not retrive user credendials.");
                return new Credentials(clientId, dataEncoding.Decode(clientSecret));
            }

            return null;
        }
    }
}
