using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation.Controllers;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Test.Presentation
{
    [TestFixture]
    public class BpsElementControllerTest
    {
        [Test]
        public async Task ShouldStartElement()
        {
            var bpsCommandService = new Moq.Mock<IBpsStartElementService>();
            bpsCommandService.Setup(x => x.Start(It.IsAny<IEnumerable<KeyValuePair<Guid, object>>>(), It.IsAny<Guid>())).Returns(
                Task.FromResult(new StartElementResult { Id = 1000, Number = "P/30", Status = "MovedToTheNextStep" }));
            BpsElementController controller = new BpsElementController(bpsCommandService.Object, null);
            var fields = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "fieldtest", new Microsoft.Extensions.Primitives.StringValues("fieldTest") },
                { "formGuid", new Microsoft.Extensions.Primitives.StringValues(Guid.NewGuid().ToString()) }
            };
            var form = new FormCollection(fields);
            var mockTempData = new Mock<ITempDataDictionary>();
            controller.TempData = mockTempData.Object;
            var result = await controller.Start(form, "");
            Assert.IsNotNull(result);
            var model = result.Value as StartElementViewModel;
            Assert.IsNotNull(model);
            Assert.IsTrue(model.ElementId == 1000 && model.ElementNumber == "P/30");
        }
        [Test]
        public async Task ShouldNotStartElement()
        {
            var bpsCommandService = new Moq.Mock<IBpsStartElementService>();
            bpsCommandService.Setup(x => x.Start(It.IsAny<IEnumerable<KeyValuePair<Guid, object>>>(), It.IsAny<Guid>())).Throws(new Exception("test error"));
            BpsElementController controller = new BpsElementController(bpsCommandService.Object, null);
            var fields = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "fieldtest", new Microsoft.Extensions.Primitives.StringValues("fieldTest") },
                { "formGuid", new Microsoft.Extensions.Primitives.StringValues(Guid.NewGuid().ToString()) }
            };
            var form = new FormCollection(fields);
            var mockTempData = new Mock<ITempDataDictionary>();
            controller.TempData = mockTempData.Object;
            var result = await controller.Start(form, "");
            Assert.AreEqual("test error", result.Value.ToString());
        }
        [Test]
        public async Task ShouldNotStartElementBecauseCollectionIsNull()
        {
            var bpsCommandService = new Moq.Mock<IBpsStartElementService>();
            BpsElementController controller = new BpsElementController(bpsCommandService.Object, null);
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            controller.TempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var result = await controller.Start(null, "");
            Assert.AreEqual("Could not get form body", controller.TempData["Error"]);
            Assert.AreEqual("Could not get form body", result.Value.ToString());
        }
        [Test]
        public async Task ShouldNotStartElementBecauseFormGuidIsNotProvided()
        {
            var bpsCommandService = new Moq.Mock<IBpsStartElementService>();
            BpsElementController controller = new BpsElementController(bpsCommandService.Object, null);
            var fields = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "fieldtest", new Microsoft.Extensions.Primitives.StringValues("fieldTest") },
            };
            var form = new FormCollection(fields);

            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            controller.TempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var result = await controller.Start(form,"");
            Assert.AreEqual("Form guid not provided", controller.TempData["Error"]);
            Assert.AreEqual("Form guid not provided", result.Value.ToString());
        }

    }
}
