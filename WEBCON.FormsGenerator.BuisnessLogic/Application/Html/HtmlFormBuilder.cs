using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

[assembly: InternalsVisibleTo("WEBCON.FormsGenerator.Test")]
namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html
{
    public class HtmlFormBuilder : IFormBuilder
    {
        public string EmbedFieldsInForm(string fields)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($@"<form id=""generated-form"">");
            builder.AppendLine(fields);
            builder.AppendLine(@$"<input type=""submit"" value=""{FormParameters.ContentSubmitText}"">");
            builder.AppendLine("</form>");
            return builder.ToString();
        }
        public string CreateFormFieldBasedOnBpsField(DTO.FormField bpsFormField)
        {
            if (bpsFormField == null) throw new ApplicationArgumentException("Cannot create field because field data has not been provided");
            if (bpsFormField is HtmlBaseField htmlField)
                return htmlField.CreateInput();
            else
                return "";
        }
        public string CreateLabel(string labelId, string labelName)
        {
            return @$"<label id=""{labelId}"">{labelName}</label>";
        }
        public string CreateFormNameArea(string formName)
        {
            return $"<h4>{formName}</h4>";
        }
        public string CreateFormDescriptionArea(string formDescription)
        {
            return $"<p>{formDescription}</p>";
        }
        public string EmbedInputsInGroup(IEnumerable<string> inputs, string groupId)
        {
            if (!inputs?.Any() ?? true) return "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(@$"<div id=""group-{groupId}"">");
            foreach (string control in inputs)
                stringBuilder.AppendLine(control);
            stringBuilder.AppendLine("</div>");

            return stringBuilder.ToString();
        }
        public string TransformMetadataOnForm(IEnumerable<FormContentField> formContentFields, string formBody, string contentTitle, string contentDescription = "", string contentSubmitText = "Submit")
        {
            string line;
            if (string.IsNullOrEmpty(formBody)) return string.Empty;
            if (formContentFields == null) return formBody;

            formBody = formBody.Replace(FormParameters.ContentTitle, contentTitle)
                .Replace(FormParameters.ContentDescription, contentDescription)
                .Replace(FormParameters.ContentSubmitText, contentSubmitText);

            var bodySplit = GetSplitedFormContent(formBody); 
            foreach (var contentField in formContentFields)
            {
                line = bodySplit.FirstOrDefault(x => x.Contains("{" + contentField.Name + "}"));
                if (line == null) continue;
                line = line.Replace("{" + contentField.Name + "}", contentField.CustomName);
                bodySplit[bodySplit.FindIndex(x => x.Contains("{" + contentField.Name + "}"))] = line;
                string fieldGuid = contentField.Name.Replace(FormParameters.FormContentFieldNamePrefix, string.Empty);
                ReplaceParameterInFormContent(FormParameters.IsRequired, contentField.IsRequired ? "required" : null, bodySplit, fieldGuid);
                ReplaceParameterInFormContent(FormParameters.RequiredText, !string.IsNullOrEmpty(contentField.CustomRequiredWarningText) ? @$"data-val=""true"" data-val-required=""{contentField.CustomRequiredWarningText}""" : null, bodySplit, fieldGuid);
                ReplaceParameterInFormContent(FormParameters.AllowMultipleValues, $" data-bpsmultivalue={contentField.AllowMultipleValues.ToString().ToLower()}" , bodySplit, fieldGuid);
            }
            var joinedBody = GetJoinedBody(bodySplit);
            return joinedBody;
        }
        private void ReplaceParameterInFormContent(string parameter, string replaceWith, List<string> formContentElementsList, string fieldGuid)
        {
            string line = formContentElementsList.FirstOrDefault(x => x.Replace(" ", string.Empty).Contains($@"name=""{fieldGuid}""") && x.Contains(parameter));
            if (line == null) return;
            line = line.Replace(parameter, replaceWith);
            int lineIndex = formContentElementsList.FindIndex(x => x.Replace(" ", string.Empty).Contains($@"name=""{fieldGuid}""") && x.Contains(parameter));
            if(lineIndex>0) formContentElementsList[formContentElementsList.FindIndex(x => x.Replace(" ", string.Empty).Contains($@"name=""{fieldGuid}""") && x.Contains(parameter))] = line;
        }
        public string EmbedInputsInExistingForm(IEnumerable<string> inputs, string existingFormBody)
        {
            if (inputs == null || !inputs.Any()) return existingFormBody;
            StringBuilder builder = new StringBuilder();
            foreach (string input in inputs)
            {
                builder.AppendLine(input);
            }
            var splited = GetSplitedFormContent(existingFormBody);
            var index = splited.FindIndex(x => x.ToLower().Contains(@$"type=""submit"""));
            splited.Insert(index, builder.ToString().Substring(1));

            return GetJoinedBody(splited);
        }
        public string RemoveFieldFromExistingForm(Guid formContentFieldGuid, string formContent)
        {
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(formContent);
            var fieldNode = html.DocumentNode.SelectSingleNode($"//div[@id='group-{formContentFieldGuid}']");
            if (fieldNode == null)
            {
                fieldNode = html.GetElementbyId(formContentFieldGuid.ToString());
                if (fieldNode == null) return formContent;
                if (fieldNode.ParentNode != null)
                {
                    var elementWithSameGuid = fieldNode.ParentNode.ChildNodes.Where(x => x.Attributes.Any(x => x.Name == "id" && x.Value != formContentFieldGuid.ToString() && x.Value.Contains(formContentFieldGuid.ToString()))).ToList();
                    elementWithSameGuid?.ForEach(x => x.Remove());
                    RemoveParentNode(fieldNode.ParentNode);
                }
            }
            fieldNode.Remove();
            return ParseParametersFromHtmlAgilityOutput(html.DocumentNode.OuterHtml);
        }

        public string UpdateField(Guid formContentFieldGuid, string formContent, string updatedHtml)
        {          
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(formContent);
            var fieldNode = html.DocumentNode.SelectSingleNode($"//div[@id='group-{formContentFieldGuid}']");
            if (fieldNode != null)
            {
                var newNode = HtmlNode.CreateNode(updatedHtml);
                fieldNode.ParentNode.ReplaceChild(newNode, fieldNode);
            }
            return ParseParametersFromHtmlAgilityOutput(html.DocumentNode.OuterHtml);
        }


        private void RemoveParentNode(HtmlNode parent)
        {
            if (parent == null) return;

            if (parent.ParentNode != null && parent.ParentNode.ChildNodes.Count <= 1)
                RemoveParentNode(parent.ParentNode);
            else
                parent.Remove();
        }
        public string UpdateChoices(DTO.FormField formField, string formContent)
        {
            if (formField == null) return formContent;
            if (!(formField is IChoice choicable)) return formContent;
            return ParseParametersFromHtmlAgilityOutput(choicable.UpdateChoicesOnFormContent(formContent));
        }

        private string ParseParametersFromHtmlAgilityOutput(string html)
        {
           return html?.Replace(@$"{FormParameters.IsRequired.ToLower()}=""""", FormParameters.IsRequired).Replace(@$"{FormParameters.RequiredText.ToLower()}=""""", FormParameters.RequiredText).Replace(@$"{FormParameters.AllowMultipleValues.ToLower()}=""""", FormParameters.AllowMultipleValues);
        }

        private List<string> GetSplitedFormContent(string formBody)
        {
            return formBody.Split("<").ToList();
        }
        private string GetJoinedBody(List<string> formBody)
        {
            return string.Join("<", formBody);
        }
    }
}