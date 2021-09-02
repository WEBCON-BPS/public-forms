using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class FormContentServiceTest
    {
        [Test]
        public void ShouldCreateFormContent()
        {
            var builderMoq = new Moq.Mock<IFormBuilder>();
            builderMoq.Setup(x => x.CreateFormFieldBasedOnBpsField(It.IsAny<FormsGenerator.BusinessLogic.Application.DTO.FormField>())).Returns(@"<input class=""form-control"" type=""text"" id="""" name="""" >");
            builderMoq.Setup(x => x.EmbedInputsInGroup(It.IsAny<string[]>(), It.IsAny<string>())).Returns(@"<div id=""group-g""><input class=""form-control"" type=""text"" id="""" name="""" ></div>");
            builderMoq.Setup(x => x.EmbedFieldsInForm(It.IsAny<string>())).Returns(@"<form><div><input class=""form-control"" type=""text"" id="""" name=""""></div></form>");
            builderMoq.Setup(x => x.CreateFormNameArea(It.IsAny<string>())).Returns("<h4>{label_formName}</h4>");
            builderMoq.Setup(x => x.CreateFormDescriptionArea(It.IsAny<string>())).Returns("<p>{label_formDescription}</p>");

            FormContentService service = new FormContentService(builderMoq.Object);

            FormContent formBody = service.CreateFormContent(new List<FormsGenerator.BusinessLogic.Application.DTO.FormField>
            {
                new FormsGenerator.BusinessLogic.Application.DTO.FormField() { Guid  = Guid.Empty, IsRequired = false, Name = "Status" },
                new FormsGenerator.BusinessLogic.Application.DTO.FormField() { Guid  = Guid.Empty, IsRequired = false, Name = "Message"},

            }, @"Form\Preview");

          //  Assert.IsTrue(formBody.Metadata.Count == 4);
         //   Assert.IsTrue(formBody.Metadata.Any(x => x.FieldDescription == "Form name text" && x.FieldId == "label_formName" && x.FieldValue == ""));
         //   Assert.IsTrue(formBody.Metadata.Any(x => x.FieldDescription == "Form description" && x.FieldId == "label_formDescription" && x.FieldValue == ""));
         //   Assert.IsTrue(formBody.Metadata.Any(x => x.FieldDescription == "Status label text" && x.FieldId == "label_" + Guid.Empty + "_" + "Status" && x.FieldValue == "Status"));
         //   Assert.IsTrue(formBody.Metadata.Any(x => x.FieldDescription == "Message label text" && x.FieldId == "label_" + Guid.Empty + "_" + "Message" && x.FieldValue == "Message"));
         //   Assert.AreEqual(@"<h4>{label_formName}</h4><p>{label_formDescription}</p><form><div><input class=""form-control"" type=""text"" id="""" name=""""></div></form>".Trim(), formBody.HtmlFormWithMetadata.Replace("\r\n", string.Empty).Trim());
        }
        [Test]
        public void ShouldGetFormBodyWithTransformedMetadata()
        {
            var builderMoq = new Moq.Mock<IFormBuilder>();
            builderMoq.Setup(x => x.TransformMetadataOnForm(It.IsAny<IEnumerable<FormContentField>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(@"<form>test full form</form>");
            FormContentService service = new FormContentService(builderMoq.Object);
            Assert.AreEqual(@"<form>test full form</form>", service.GetFormContentWithTransformedMetadata(new List<FormContentField>(), "","","",""));
        }
    }
}
