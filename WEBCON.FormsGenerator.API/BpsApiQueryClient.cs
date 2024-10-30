using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.API.ApiModels;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using System.Data;

namespace WEBCON.FormsGenerator.API
{
    public class BpsApiQueryClient : ApiBaseClient, IBpsClientQueryService
    {
        private readonly int _databaseId;
        private readonly IFormFieldFactory _fieldFactory;
        private string _apiMainPath = "";

        public BpsApiQueryClient(string apiUrl, int databaseId, IFormFieldFactory fieldFactory, Credentials credentials) : base(apiUrl, credentials)
        {
            _apiMainPath = $"/api/data/v6.0/db/{(databaseId.Equals(0) ? 1 : databaseId)}";
            _databaseId = databaseId;
            _fieldFactory = fieldFactory;
        }

        public Task<string> Authenticate()
        {
            return base.Authenticate();
        }

        public async Task<IEnumerable<BpsApplication>> GetApplicationsAsync()
        {
            return await GetApiMetadata<BpsApplication>($"applications", (response) => { return JsonSerializer.Deserialize<Applications>(response).applications; });
        }

        public async Task<IEnumerable<BpsProcess>> GetProcessesAsync(Guid applicationGuid)
        {
            return await GetApiMetadata<BpsProcess>($"applications/{applicationGuid}/processes", (response) => { return JsonSerializer.Deserialize<Processes>(response).processes; });
        }

        public async Task<IEnumerable<BpsWorkflow>> GetWorkflowsAsync(Guid processGuid)
        {
            return await GetApiMetadata<BpsWorkflow>($"processes/{processGuid}/workflows", (response) => { return JsonSerializer.Deserialize<Workflows>(response).workflows; });
        }

        public async Task<IEnumerable<BpsFormType>> GetAssociatedFormTypesAsync(Guid workflowGuid)
        {
            return await GetApiMetadata<BpsFormType>($"workflows/{workflowGuid}/associatedFormTypes", (response) => { return JsonSerializer.Deserialize<FormTypes>(response).associatedFormTypes; });
        }

        public async Task<BpsStep> GetStartStepAsync(Guid workflowGuid)
        {
            var steps = await GetApiMetadata<BpsStep>($"workflows/{workflowGuid}/steps", (response) => { return JsonSerializer.Deserialize<Steps>(response).steps; });
            return await FindStartStepAsync(steps);
        }

        private async Task<BpsStep> FindStartStepAsync(IEnumerable<BpsStep> steps)
        {
            if(steps != null)
            foreach (var step in steps)
            {
                var response = await SendRequestAsync(CreateRequestFullUrl($"{_apiMainPath}/steps/{step.Guid}"), null, "GET");
                if (response.status == HttpStatusCode.OK)
                    if (JsonSerializer.Deserialize<BpsStep>(response.response).Type.Equals("Start"))
                        return step;              
            }
            return null;
        }

        public async Task<IEnumerable<BpsPath>> GetStepPaths(Guid stepGuid)
        {
            return await GetApiMetadata<BpsPath>($"steps/{stepGuid}/paths", (response) => { return JsonSerializer.Deserialize<Paths>(response).paths; });
        }

        public async Task<IEnumerable<BpsBusinessEntity>> GetBusinessEntities()
        {
            _apiMainPath = $"/api/data/v6.0/admin/db/{_databaseId}";
            return await GetApiMetadata<BpsBusinessEntity>("businessentities", (response) => { return JsonSerializer.Deserialize<BusinessEntities>(response).businessEntities; });
        }

        private async Task<IEnumerable<T>> GetApiMetadata<T>(string endpoint, Func<string, IEnumerable<BaseModel>> createResultAction) where T : BaseObject, new()
        {
            var response = await SendRequestAsync(CreateRequestFullUrl($"{_apiMainPath}/{endpoint}"), null, "GET");
            if (response.status == HttpStatusCode.OK)
            {
                var result = createResultAction(response.response);
                return result.Select(x => new T
                {
                    Guid = new Guid(x.guid),
                    Name = x.name,
                }).ToList();
            }
            else
                throw CreateBpsClientDataException(response.response, response.status);
        }

        public async Task<IEnumerable<BusinessLogic.Application.DTO.FormField>> GetFormFieldsAsync(Guid formTypeGuid, Guid workflowGuid, Guid stepGuid, string token = "")
        {
            (Field[] newElementFields, FormLayoutField[] formLayoutFields) result = await GetFormFields(formTypeGuid, workflowGuid, stepGuid, token);
            return result.newElementFields?.Select(x => _fieldFactory.FormFieldBuilder.GetFormField(new Guid(x.guid), x.name, _fieldFactory.FormValueBuilder.GetFormFieldValue(x.type, null), result.formLayoutFields?.Where(z => z.guid == x.guid).FirstOrDefault()?.configuration?.allowMultipleValues ?? false, result.formLayoutFields?.Where(z => z.guid == x.guid).FirstOrDefault()?.requiredness?.Equals("Mandatory") ?? false , x.type?.ToLower().Equals("autocomplete") ?? false)).ToList();            
        }

        public async IAsyncEnumerable<BusinessLogic.Application.DTO.FormField> GetFormFieldsWithValuesAsync(Guid formTypeGuid, Guid workflowGuid, Guid stepGuid, string token = "")
        {
            (Field[] newElementFields, FormLayoutField[] formLayoutFields) = await GetFormFields(formTypeGuid, workflowGuid, stepGuid, token);

            if (newElementFields == null) 
                yield break;

            foreach(var field in newElementFields)
            {
                var layoutField = formLayoutFields.Single(f => f.guid.Equals(field.guid, StringComparison.InvariantCultureIgnoreCase));

                switch (field.type)
                {
                    case "ChoiceList":
                    case "Autocomplete":
                    case "ChoicePicker":
                    case "People":
                        {
                            var value = _fieldFactory.FormValueBuilder.GetFormFieldValue(field.type, await GetChoiceListValues(field.guid, formTypeGuid, workflowGuid));
                            yield return CreateField(field, layoutField, value);                            

                            break;
                        }

                    case "SurveyChoose":
                        {
                            var value = _fieldFactory.FormValueBuilder.GetFormFieldValue(field.type, layoutField.configuration.answers);
                            yield return CreateField(field, layoutField, value);

                            break;
                        }

                    case "RatingScale":
                        {
                            var value = _fieldFactory.FormValueBuilder.GetFormFieldValue(field.type, layoutField.configuration.scaleValues);
                            yield return CreateField(field, layoutField, value);

                            break;
                        }

                    default:
                        {
                            var value = _fieldFactory.FormValueBuilder.GetFormFieldValue(field.type, field.value);
                            yield return CreateField(field, layoutField, value);

                            break;
                        }
                }
            }
        }

        private BusinessLogic.Application.DTO.FormField CreateField(Field field, FormLayoutField layoutField, BusinessLogic.Domain.Model.FieldValue value)
        {
            return _fieldFactory.FormFieldBuilder.GetFormField(
                                            new Guid(field.guid),
                                            field.name,
                                            value,
                                            layoutField.configuration?.allowMultipleValues ?? false,
                                            layoutField.requiredness?.Equals("mandatory", StringComparison.InvariantCultureIgnoreCase) ?? false,
                                            field.formLayout.editability.Equals("ReadOnly", StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<FormFieldValueData[]> GetChoiceListValues(string guid,  Guid formTypeGuid, Guid workflowGuid)
        {
            var choiceListValuesResponse = await SendRequestAsync(CreateRequestFullUrl($"{_apiMainPath}/elements/resolveFieldValue/{guid}?workflow={workflowGuid}&formType={formTypeGuid}"), JsonSerializer.Serialize(new { }), "POST");
            if (choiceListValuesResponse.status == HttpStatusCode.OK)
            {
                var value =  Newtonsoft.Json.JsonConvert.DeserializeObject<FormFieldValue>(choiceListValuesResponse.response);
                return MapValuesToObjects(value);
            }

            return null;
        }

        private FormFieldValueData[] MapValuesToObjects(FormFieldValue formFieldValue)
        {
            var idColumn = formFieldValue.columns.Single(c => c.type.Equals("Id", StringComparison.InvariantCulture)).sourceColumnName;
            var nameColumn = formFieldValue.columns.Single(c => c.type.Equals("Name", StringComparison.InvariantCulture)).sourceColumnName;

            return formFieldValue.data.AsEnumerable().Select(r => new FormFieldValueData()
            {
                id = r[idColumn]?.ToString(),
                name = r[nameColumn]?.ToString()
            }).ToArray();
        }

        private async Task<(Field[] newElementFields, FormLayoutField[] formLayoutFields)> GetFormFields(Guid formTypeGuid, Guid workflowGuid, Guid stepGuid, string token = "")
        {
            OverrideCurrentToken(token);
            var elementsNewResponse = await SendRequestAsync(CreateRequestFullUrl($"{_apiMainPath}/elements/new?workflow={workflowGuid}&formtype={formTypeGuid}&expand=formLayout"), null, "GET");
            if (elementsNewResponse.status == HttpStatusCode.OK)
            {
                var elementNewResult = JsonSerializer.Deserialize<NewElement>(elementsNewResponse.response).formFields;
                if (elementNewResult == null) return (null, null);

                var formLayoutResponse = await SendRequestAsync(CreateRequestFullUrl($"{_apiMainPath}/formlayout?step={stepGuid}&formType={formTypeGuid}"), null, "GET");

                if (formLayoutResponse.status == HttpStatusCode.OK)
                    return (elementNewResult, JsonSerializer.Deserialize<FormLayouts>(formLayoutResponse.response).fields);
                else
                    throw CreateBpsClientDataException(formLayoutResponse.response, formLayoutResponse.status);
            }
            else
                throw CreateBpsClientDataException(elementsNewResponse.response, elementsNewResponse.status);
        }
    }
}
