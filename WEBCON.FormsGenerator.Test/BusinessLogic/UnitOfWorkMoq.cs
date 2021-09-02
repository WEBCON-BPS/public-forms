using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    static class UnitOfWorkMoq
    {
        public static Moq.Mock<IFormUnitOfWork> Get()
        {
            var moq = new Moq.Mock<IFormUnitOfWork>();
            var formR = new Moq.Mock<IFormRepository>();
            var appR = new Moq.Mock<IRepository<BpsApplication>>();
            var wfR = new Moq.Mock<IRepository<BpsWorkflow>>();
            var procR = new Moq.Mock<IRepository<BpsProcess>>();
            var ftR = new Moq.Mock<IRepository<BpsFormType>>();
            var fieldsR = new Moq.Mock<IRepository<BpsFormField>>();
            var stepR = new Moq.Mock<IRepository<BpsWorkflowStep>>();
            var pathR = new Moq.Mock<IRepository<BpsStepPath>>();
            var metaR = new Moq.Mock<IRepository<FormContentField>>();
            var entities = new Moq.Mock<IRepository<BpsBusinessEntity>>();
            moq.Setup(x => x.BpsApplications).Returns(appR.Object);
            moq.Setup(x => x.BpsFormTypes).Returns(ftR.Object);
            moq.Setup(x => x.BpsProcesses).Returns(procR.Object);
            moq.Setup(x => x.BpsWorkflows).Returns(wfR.Object);
            moq.Setup(x => x.Forms).Returns(formR.Object);
            moq.Setup(x => x.BpsFormFields).Returns(fieldsR.Object);
            moq.Setup(x => x.BpsWorkflowSteps).Returns(stepR.Object);
            moq.Setup(x => x.BpsStepPaths).Returns(pathR.Object);
            moq.Setup(x => x.FormContentFields).Returns(metaR.Object);
            moq.Setup(x => x.BpsBusinessEntity).Returns(entities.Object);
            return moq;
        }
    }
}
