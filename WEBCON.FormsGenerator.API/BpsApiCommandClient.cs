using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.API.ApiModels;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.API.FormField;

namespace WEBCON.FormsGenerator.API
{
    public class BpsApiCommandClient : ApiBaseClient, IBpsClientCommandService
    {
        private readonly int databaseId;
        private readonly string apiMainPath = "";

        public BpsApiCommandClient(string apiUrl, int databaseId, Credentials credentials) : base(apiUrl, credentials)
        {
            this.databaseId = databaseId.Equals(0) ? 1 : databaseId;
            apiMainPath = $"/api/data/beta/db/{databaseId}";
        }    
        public async Task<BusinessLogic.Application.DTO.StartElementResult> StartElement(IEnumerable<BusinessLogic.Application.DTO.FormField> fields, Guid workflowGuid, Guid formTypeGuid, Guid stepPathGuid, Guid businessEntityGuid, Credentials customCredentials = null)
        {
            List<StartElementFormField> startFields = new List<StartElementFormField>();
            if(!(fields.Cast<BpsBaseField>() is IEnumerable<BpsBaseField> bpsFields))
                throw new ApplicationArgumentException("Provided form fields have incorrect BPS fields type");

            foreach (var field in bpsFields)
            {
                if (field.Value == null)
                    throw new ApplicationArgumentException($"Field value has to be provided (field {field.Name} ({field.Guid}))");

                object fieldValue = field.ValueToBps();

                startFields.Add(new StartElementFormField
                {
                    guid = field.Guid.ToString(),
                    value = fieldValue
                });
            }
            IdentityModel businessEntity = (businessEntityGuid != Guid.Empty) ? new IdentityModel() { guid = businessEntityGuid.ToString() } : null;
            StartElement element = new StartElement
            {
                businessEntity = businessEntity,
                formType = new IdentityModel { guid = formTypeGuid.ToString() },
                parentInstanceId = null,
                workflow = new IdentityModel { guid = workflowGuid.ToString() },
                formFields = startFields.ToArray()
            };
            string requestData = JsonSerializer.Serialize(element, new JsonSerializerOptions() { IgnoreNullValues = true });
            var requestUrl = CreateRequestFullUrl($"{apiMainPath}/elements?db={databaseId}&path={((stepPathGuid != Guid.Empty) ? stepPathGuid.ToString() : "default")}");

            (string response, HttpStatusCode status) response;

            if (customCredentials == null)
                response = await SendRequestAsync(requestUrl, requestData, "POST");
            else
                response = await SendRequestWithCustomCredentialsAsync(requestUrl, requestData, customCredentials, "POST");

            if (response.status == HttpStatusCode.OK)
            {
                var result = JsonSerializer.Deserialize<ApiModels.StartElementResult>(response.response);
                return new BusinessLogic.Application.DTO.StartElementResult
                {
                    Id = result.id,
                    Number = result.instanceNumber,
                    Status = result.status
                };
            }
            else
                throw CreateBpsClientDataException(response.response, response.status);
        }      
    }
}
