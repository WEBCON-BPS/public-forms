using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class BpsQueryServiceTest
    {
        [Test]
        public async Task ShouldGetApplications()
        {
            var clientService = new Moq.Mock<IBpsClientQueryService>();
            Guid appGuid = Guid.NewGuid();
            clientService.Setup(x => x.GetApplicationsAsync()).Returns(Task.FromResult(new List<BpsApplication>
                {
                    new BpsApplication{ Guid = appGuid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsQueryService bpsFormService = new BpsQueryService(clientService.Object);
            var result = await bpsFormService.GetApplicationsAsync();
            BpsApplication application = result.First();
            Assert.IsTrue(application.Name == "Form Contact" && application.Guid == appGuid);
        }
        [Test]
        public async Task ShouldGetProcess()
        {
            var clientService = new Moq.Mock<IBpsClientQueryService>();
            var formBuilder = new Moq.Mock<IFormBuilder>();
            Guid appGuid = Guid.NewGuid();
            clientService.Setup(x => x.GetProcessesAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new List<BpsProcess>
                {
                    new BpsProcess{ Guid = appGuid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsQueryService bpsFormService = new BpsQueryService(clientService.Object);
            var result = await bpsFormService.GetProcessesAsync(appGuid);
            BpsProcess process = result.First();
            Assert.IsTrue(process.Name == "Form Contact" && process.Guid == appGuid);
        }
        [Test]
        public async Task ShouldGetWorkflow()
        {
            var clientService = new Moq.Mock<IBpsClientQueryService>();
            Guid wfGuid = Guid.NewGuid();
            clientService.Setup(x => x.GetWorkflowsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new List<BpsWorkflow>
                {
                    new BpsWorkflow{ Guid = wfGuid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsQueryService bpsFormService = new BpsQueryService(clientService.Object);
            var result = await bpsFormService.GetWorkflowsAsync(wfGuid);
            BpsWorkflow wf = result.First();
            Assert.IsTrue(wf.Name == "Form Contact" && wf.Guid == wfGuid);
        }
        [Test]
        public async Task ShouldGetFormType()
        {
            var clientService = new Moq.Mock<IBpsClientQueryService>();
            var encoding = new Moq.Mock<IDataEncoding>();
            Guid appGuid = Guid.NewGuid();
            clientService.Setup(x => x.GetAssociatedFormTypesAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new List<BpsFormType>
                {
                    new BpsFormType{ Guid = appGuid, Name = "Form Contact"}
                }.AsEnumerable()));
            BpsQueryService bpsFormService = new BpsQueryService(clientService.Object);
            var result = await bpsFormService.GetAssociatedFormTypesAsync(appGuid);
            BpsFormType ft = result.First();
            Assert.IsTrue(ft.Name == "Form Contact" && ft.Guid == appGuid);
        }
        [Test]
        public async Task ShouldGetStartStep()
        {
            var clientService = new Moq.Mock<IBpsClientQueryService>();
            var encoding = new Moq.Mock<IDataEncoding>();
            Guid appGuid = Guid.NewGuid();
            clientService.Setup(x => x.GetStartStepAsync(It.IsAny<Guid>())).Returns(Task.FromResult(
                    new BpsStep{ Guid = appGuid, Name = "Form Contact"}));
            BpsQueryService bpsFormService = new BpsQueryService(clientService.Object);
            var result = await bpsFormService.GetStartStepAsync(appGuid);
            Assert.IsTrue(result.Name == "Form Contact" && result.Guid == appGuid);
        }
        [Test]
        public async Task ShouldGetPath()
        {
            var clientService = new Moq.Mock<IBpsClientQueryService>();
            var encoding = new Moq.Mock<IDataEncoding>();
            Guid stepGuid = Guid.NewGuid();
            clientService.Setup(x => x.GetStepPaths(It.IsAny<Guid>())).Returns(Task.FromResult(
                    new List<BpsPath> { new BpsPath { Guid = stepGuid, Name = "Form Contact" } }.AsEnumerable()));
            BpsQueryService bpsFormService = new BpsQueryService(clientService.Object);
            var result = await bpsFormService.GetStepPathsAsync(stepGuid);
            Assert.IsTrue(result.First().Name == "Form Contact" && result.First().Guid == stepGuid);
        }
    }
}
