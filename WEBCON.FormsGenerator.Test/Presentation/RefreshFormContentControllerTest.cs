using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation.Controllers;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Test.Presentation
{
    [TestFixture]
    public class RefreshFormContentControllerTest
    {
        [Test]
        public async Task ShouldRefreshContent()
        {
            var refreshContentServiceMoq = new Moq.Mock<IFormContentRefreshService>();
            refreshContentServiceMoq.Setup(x => x.RefreshFormContentAsync(It.IsAny<IEnumerable<FormField>>(), It.IsAny<Guid>())).Returns
                (Task.FromResult(new FormContent
                {
                    ContentFields = new List<FormContentField> { new FormContentField { CustomName = "Name", IsRequired = true, Guid = Guid.NewGuid() } },
                    FormContentTransformed = "<form></form>",
                    FormContentWithMetadata = "<form>{isRequired}</form>"
                }));
            var bpsQueryServiceMoq = new Moq.Mock<IBpsQueryService>();
            FormContentRefreshController controller = new FormContentRefreshController(refreshContentServiceMoq.Object, bpsQueryServiceMoq.Object, null, null);
            var result = await controller.RefreshContent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var content = result.Value as FormContentViewModel;
            Assert.IsNotNull(content);
            Assert.AreEqual("<form></form>", content.FormContentTransformed);
            Assert.AreEqual("<form>{isRequired}</form>", content.FormContentWithMetadata);
            Assert.AreEqual("Name", content.ContentFields.FirstOrDefault().CustomName);
        }
        [Test]
        public async Task ShouldNotRefreshContent()
        {
            var refreshContentServiceMoq = new Moq.Mock<IFormContentRefreshService>();
            refreshContentServiceMoq.Setup(x => x.RefreshFormContentAsync(It.IsAny<IEnumerable<FormField>>(), It.IsAny<Guid>())).Throws(new Exception("test error"));
            var bpsQueryServiceMoq = new Moq.Mock<IBpsQueryService>();
            FormContentRefreshController controller = new FormContentRefreshController(refreshContentServiceMoq.Object, bpsQueryServiceMoq.Object, null, null);
            var result = await controller.RefreshContent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            Assert.AreEqual("test error", result.Value.ToString());
        }
    }
}
