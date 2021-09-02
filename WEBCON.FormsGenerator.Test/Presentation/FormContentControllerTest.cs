using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation.Controllers;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Test.Presentation
{
    [TestFixture]
    public class FormContentControllerTest
    {
        [Test]
        public async Task ShouldGetFormTypeBody()
        {
            var formContentService = new Moq.Mock<IFormContentService>();
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            Guid guid = Guid.NewGuid();
            var expected = new FormContentViewModel
            {
                FormContentTransformed = "<form></form>",
                FormContentWithMetadata = "<form></form>",
                ContentFields = new List<FormContentFieldViewModel> { new FormContentFieldViewModel { Name = "test_id", CustomName = "test" } }
            };
            formContentService.Setup(x => x.CreateFormContent(It.IsAny<IEnumerable<FormField>>(), It.IsAny<string>())).Returns(new FormContent
            {
                FormContentTransformed = "<form></form>",
                FormContentWithMetadata = "<form></form>",
                ContentFields = new List<FormContentField> { new FormContentField { Name = "test_id", CustomName = "test" } }
            }); 
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<FormContent, FormContentViewModel>(It.IsAny<FormContent>())).Returns(expected);
            FormContentController controller = new FormContentController(formContentService.Object, bpsQueryService.Object, null, mapperMock.Object, null);
            var result = await controller.Create(Guid.NewGuid(),"", Guid.NewGuid(), Guid.NewGuid());
            FormContentViewModel resultBody = result.Value as FormContentViewModel;
            Assert.IsNotNull(resultBody);
            Assert.AreEqual(expected.FormContentTransformed, resultBody.FormContentTransformed);
            Assert.AreEqual(expected.FormContentWithMetadata, resultBody.FormContentWithMetadata);
            Assert.AreEqual(expected.ContentFields.First().Name, expected.ContentFields.First().Name);
        }
        [Test]
        public async Task ShouldNotReturnFormTypeBody()
        {
            var formContentService = new Moq.Mock<IFormContentService>();
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            formContentService.Setup(x => x.CreateFormContent(It.IsAny<IEnumerable<FormField>>(), It.IsAny<string>())).Throws(new Exception("test error"));
            FormContentController controller = new FormContentController(formContentService.Object, bpsQueryService.Object, null,null, null);
            var result = await controller.Create(Guid.NewGuid(),"", Guid.NewGuid(), Guid.NewGuid());
            Assert.AreEqual("test error", result.Value);
        }
        [Test]
        public async Task ShouldReturnFormContentWithTransformedMetadata()
        {
            var formContentService = new Moq.Mock<IFormContentService>();
            formContentService.Setup(x => x.GetFormContentWithTransformedMetadataAsync(It.IsAny<IEnumerable<FormContentField>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("<form>test form content</form>"));
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<List<FormContentFieldViewModel>, List<FormContentField>>(It.IsAny<List<FormContentFieldViewModel>>()));
            FormContentController controller = new FormContentController(formContentService.Object, null, null, mapperMock.Object,null);
            var result = await controller.GetWithTransformedMeta(new List<FormContentFieldViewModel>(), "", "", "", "");
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(@"""{formContentTransformed=<form>test form content</form>}""".Replace(" ",null), JsonSerializer.Serialize(jsonResult.Value.ToString()).Replace("\\u003C", "<").Replace("\\u003E", ">").Replace(" ",null));
        }
    }
}
