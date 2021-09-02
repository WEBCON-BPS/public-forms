using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation.Controllers;

namespace WEBCON.FormsGenerator.Test.Presentation
{
    [TestFixture]
    public class BpsDataControllerTest
    {
        [Test]
        public async Task ShouldGetApplications()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            Guid guid = Guid.NewGuid();
            bpsQueryService.Setup(x => x.GetApplicationsAsync()).Returns(Task.FromResult(new List<BpsApplication>
                {
                    new BpsApplication{ Guid = guid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);

            var result = await controller.GetApplications();
            var expected = new List<KeyValuePair<Guid, string>>() { new KeyValuePair<Guid, string>(guid, "Form Contact") };
            Assert.AreEqual(expected, result.Value);
        }
        [Test]
        public async Task ShouldNotReturnApplications()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            bpsQueryService.Setup(x => x.GetApplicationsAsync()).Throws(new Exception("test error"));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);
            var result = await controller.GetApplications();
            Assert.AreEqual("test error", result.Value);
        }
        [Test]
        public async Task ShouldGetProcesses()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            Guid guid = Guid.NewGuid();
            bpsQueryService.Setup(x => x.GetProcessesAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new List<BpsProcess>
                {
                    new BpsProcess{ Guid = guid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);

            var result = await controller.GetProcesses(guid);
            var expected = new List<KeyValuePair<Guid, string>>() { new KeyValuePair<Guid, string>(guid, "Form Contact") };
            Assert.AreEqual(expected, result.Value);
        }
        [Test]
        public async Task ShouldNotReturnProcesses()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            bpsQueryService.Setup(x => x.GetProcessesAsync(It.IsAny<Guid>())).Throws(new Exception("test error"));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);
            var result = await controller.GetProcesses(Guid.NewGuid());
            Assert.AreEqual("test error", result.Value);
        }
        [Test]
        public async Task ShouldGetWorkflows()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            Guid guid = Guid.NewGuid();
            bpsQueryService.Setup(x => x.GetWorkflowsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new List<BpsWorkflow>
                {
                    new BpsWorkflow { Guid = guid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);
            var result = await controller.GetWorkflows(guid);
            var expected = new List<KeyValuePair<Guid, string>>() { new KeyValuePair<Guid, string>(guid, "Form Contact") };
            Assert.AreEqual(expected, result.Value);

        }
        [Test]
        public async Task ShouldNotReturnWorkflows()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            bpsQueryService.Setup(x => x.GetWorkflowsAsync(It.IsAny<Guid>())).Throws(new Exception("test error"));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);
            var result = await controller.GetWorkflows(Guid.NewGuid());
            Assert.AreEqual("test error", result.Value);
        }
        [Test]
        public async Task ShouldGetForms()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            bpsQueryService.Setup(x => x.GetAssociatedFormTypesAsync(It.IsAny<Guid>())).Throws(new Exception("test error"));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);
            var result = await controller.GetForms(Guid.NewGuid());
            Assert.AreEqual("test error", result.Value);
        }
        [Test]
        public async Task ShouldGetStartStep()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            Guid guid = Guid.NewGuid();
            bpsQueryService.Setup(x => x.GetStartStepAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new BpsStep
            {
                Guid = guid,
                Name = "Form Contact"
            }));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);
            var result = await controller.GetStartStep(Guid.NewGuid());
            var expected = new KeyValuePair<Guid, string>(guid, "Form Contact");
            Assert.AreEqual(expected, result.Value);
        }
        [Test]
        public async Task ShouldNotReturnStartStep()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            bpsQueryService.Setup(x => x.GetStartStepAsync(It.IsAny<Guid>())).Throws(new Exception("test error"));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object,null,null);
            var result = await controller.GetStartStep(Guid.NewGuid());
            Assert.AreEqual("test error", result.Value);
        }
        [Test]
        public async Task ShouldGetPaths()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            Guid guid = Guid.NewGuid();
            bpsQueryService.Setup(x => x.GetStepPathsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new List<BpsPath>
                {
                    new BpsPath { Guid = guid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object, null,null);
            var result = await controller.GetStepPaths(guid);
            var expected = new List<KeyValuePair<Guid, string>>() { new KeyValuePair<Guid, string>(guid, "Form Contact") };
            Assert.AreEqual(expected, result.Value);
        }
        [Test]
        public async Task ShouldGetToken()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            bpsQueryService.Setup(x => x.Authenticate()).Returns(Task.FromResult("tokentesttoken"));
            BpsDataController controller = new BpsDataController(bpsQueryService.Object, null,null);
            var result = await controller.GetToken();
            Assert.AreEqual("tokentesttoken", result.Value.GetType().GetProperty("token").GetValue(result.Value));
        }
        [Test]
        public async Task ShouldThrowErrorWhenGetToken()
        {
            var bpsQueryService = new Moq.Mock<IBpsQueryService>();
            bpsQueryService.Setup(x => x.Authenticate()).Throws(new Exception("test error"));

            BpsDataController controller = new BpsDataController(bpsQueryService.Object, null,null);
            var result = await controller.GetToken();
            Assert.AreEqual("test error", result.Value);
        }
    }
}
