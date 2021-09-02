using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class FormContentRefreshServiceTest
    {
        [Test]
        public void ShouldRefreshContent()
        {
            var unitOfWorkMoq = new Moq.Mock<IFormUnitOfWork>();
            var form = new FormsGenerator.BusinessLogic.Domain.Model.Form();
            form.SetContent(@"<form><input type=""submit""><form>");
            unitOfWorkMoq.Setup(x => x.Forms.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form, bool>>>())).Returns(form);
            unitOfWorkMoq.Setup(x => x.FormContentFields.GetFiltered(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.FormContentField, bool>>>())).Returns((IEnumerable<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.FormContentField>)null);
            var formBuilderMoq = new HtmlFormBuilder();
            var formContentServiceMoq = new FormContentService(formBuilderMoq);
            var fieldGuid = Guid.NewGuid();
            FormContentRefreshService refreshFormContentService = new FormContentRefreshService(unitOfWorkMoq.Object,formContentServiceMoq, formBuilderMoq);
            var result = refreshFormContentService.RefreshFormContentAsync(new List<FormField>
            {
                new SingleLineField(new HtmlSingleLineValue("test")) { Guid = fieldGuid, IsReadonly = false, IsRequired = false, Name = "Name" }
            }, Guid.NewGuid());
            Assert.AreEqual(@$"<form><div id=""group-{fieldGuid}""><label id=""{FormParameters.FormContentFieldNamePrefix}{fieldGuid}"">"+"{"+$"{FormParameters.FormContentFieldNamePrefix}{fieldGuid}"+ "}" + @$"</label><input type=""text"" data-bpsname=""Name"" id=""{fieldGuid}"" name=""{fieldGuid}""" + @" value=""test"" {isRequired} {requiredWarningText}></div><input type=""submit""><form>", result.Result.FormContentWithMetadata.Replace("\r\n", null).Trim());
            Assert.AreEqual(@$"<form><div id=""group-{fieldGuid}""><label id=""{FormParameters.FormContentFieldNamePrefix}{fieldGuid}"">Name</label><input type=""text"" data-bpsname=""Name"" id=""{fieldGuid}"" name=""{fieldGuid}"" value=""test""  ></div><input type=""submit""><form>", result.Result.FormContentTransformed.Replace("\r\n",null).Trim());
        }
        [Test]
        public void ShouldRefreshContentWhereFieldIsAddedAndRemoved()
        {
            var unitOfWorkMoq = new Moq.Mock<IFormUnitOfWork>();
            var form = new FormsGenerator.BusinessLogic.Domain.Model.Form();
            var removedFieldGuid = Guid.NewGuid();
            var newFieldGuid = Guid.NewGuid();
            form.SetContent(@"<form><div><label id="""+FormParameters.FormContentFieldNamePrefix+$@"{removedFieldGuid}"">" + "{" + $"{FormParameters.FormContentFieldNamePrefix}{removedFieldGuid}" + "}" + @$"</label><input type=""text"" id=""{removedFieldGuid}"" name=""{removedFieldGuid}""" + @" value=""test"" {isRequired} {requiredWarningText}></div><input type=""submit""></form>");
            unitOfWorkMoq.Setup(x => x.Forms.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form, bool>>>())).Returns(form);
            unitOfWorkMoq.Setup(x => x.FormContentFields.GetFiltered(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.FormContentField, bool>>>())).Returns(new List<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.FormContentField>
            {
                new FormsGenerator.BusinessLogic.Domain.Model.FormContentField(form, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormField(removedFieldGuid, "Name",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.SingleLine, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()), "Name", "Name")
            });
            var formBuilderMoq = new HtmlFormBuilder();
            var formContentServiceMoq = new FormContentService(formBuilderMoq);
            FormContentRefreshService refreshFormContentService = new FormContentRefreshService(unitOfWorkMoq.Object, formContentServiceMoq, formBuilderMoq);
            var result = refreshFormContentService.RefreshFormContentAsync(new List<FormField>
            {
                new SingleLineField(new HtmlSingleLineValue("test2")) { Guid = newFieldGuid, IsReadonly = false, IsRequired = false, Name = "Name2" }
            }, Guid.NewGuid());
            Assert.AreEqual(@$"<form><div id=""group-{newFieldGuid}""><label id=""{FormParameters.FormContentFieldNamePrefix}{newFieldGuid}"">" + "{" + $"{FormParameters.FormContentFieldNamePrefix}{newFieldGuid}" + "}" + @$"</label><input type=""text"" data-bpsname=""Name2"" id=""{newFieldGuid}"" name=""{newFieldGuid}""" + @" value=""test2"" {isRequired} {requiredWarningText}></div><input type=""submit""></form>", result.Result.FormContentWithMetadata.Replace("\r\n", null).Trim());
            Assert.AreEqual(@$"<form><div id=""group-{newFieldGuid}""><label id=""{FormParameters.FormContentFieldNamePrefix}{newFieldGuid}"">Name2</label><input type=""text"" data-bpsname=""Name2"" id=""{newFieldGuid}"" name=""{newFieldGuid}"" value=""test2""  ></div><input type=""submit""></form>", result.Result.FormContentTransformed.Replace("\r\n", null).Trim());
        }
    }
}
