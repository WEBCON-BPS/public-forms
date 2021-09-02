using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    class FormCommandServiceTest
    {
        [Test]
        public void ShouldReturnFormsTest()
        {
           /* var moq = new Moq.Mock<IFormUnitOfWork>();
            var repoMoq = new Moq.Mock<IRepository<Form>>();
            var builderMoq = new Moq.Mock<IFormBuilder>();
            moq.Setup(x => x.Forms.GetAll()).Returns(new List<Form>
            {
                new Form{Name = "workflow"},
                new Form{Name = "process"}
            });
            FormService service = new FormService(moq.Object, builderMoq.Object);
            var result = service.GetForms(null,1,null);*/
            //Assert.IsTrue(result.Count().Equals(2));
           // Assert.IsTrue(result.First().Name.Equals("workflow"));
        }
        [Test]
        public void ShouldAddNewForm()
        {
            var moq = UnitOfWorkMoq.Get();
            moq.Setup(x => x.BpsFormFields.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.BpsFormField, bool>>>())).Returns(new BpsFormField(Guid.NewGuid(), "field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.SingleLine, new BpsFormType()));            
            FormCommandService service = new FormCommandService(moq.Object);
            service.AddForm(new FormsGenerator.BusinessLogic.Application.DTO.Form
            {
                BpsApplicationGuid = Guid.NewGuid(),
                Name = "test",
                Content = "<form></form>",
                ContentFields = new List<FormsGenerator.BusinessLogic.Application.DTO.FormContentField>
                {
                     new FormsGenerator.BusinessLogic.Application.DTO.FormContentField()
                     {
                         Name = "Name",
                         BpsFormField = new FormsGenerator.BusinessLogic.Application.DTO.FormField(){ Name = "Field 1", Guid = Guid.NewGuid(), Type = FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.SingleLine },
                } 
                 },
                BpsApplicationName = "Application name",
                BpsFormTypeGuid = Guid.NewGuid(),
                BpsFormTypeName = "Form name",
                BpsProcessGuid = Guid.NewGuid(),
                BpsProcessName = "Process name",
                BpsWorkflowGuid = Guid.NewGuid(),
                BpsWorkflowName = "Workflow name",
                BpsStartStepPathGuid = Guid.NewGuid(),
                BpsStartStepPathName = "Path name",
                BpsWorkflowStartStepGuid = Guid.NewGuid(),
                BpsWorkflowStartStepName = "Step name"
            });
        }
        [TestCase(false, true,true,true,true)]
        [TestCase(true, false, true, true, true)]
        [TestCase(true, true, false, true, true)]
        [TestCase(true, true, true, false, true)]
        [TestCase(true, true, true, true, false)]
        public void ShouldntAddNewForm(bool hasApp, bool hasProc, bool hasWf, bool hasForm, bool hasStep)
        {
            Guid appGuid = hasApp ? Guid.NewGuid() : Guid.Empty;
            Guid procGuid = hasProc ? Guid.NewGuid() : Guid.Empty;
            Guid wfGuid = hasWf ? Guid.NewGuid() : Guid.Empty;
            Guid formGuid = hasForm ? Guid.NewGuid() : Guid.Empty;
            Guid stepGuid = hasStep ? Guid.NewGuid() : Guid.Empty;

            var moq = UnitOfWorkMoq.Get().Object;
            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormCommandService service = new FormCommandService(moq);
            Assert.Throws<ApplicationArgumentException>(() => service.AddForm(new FormsGenerator.BusinessLogic.Application.DTO.Form
            {
                BpsApplicationGuid = appGuid,
                BpsFormTypeGuid = formGuid,
                BpsProcessGuid = procGuid,
                BpsWorkflowGuid = wfGuid,
                BpsWorkflowStartStepGuid = stepGuid
            }));
        }
        [Test]
        public void ShouldEditForm()
        {
            var moq = UnitOfWorkMoq.Get();
            var formR = new Moq.Mock<IFormRepository>();
            formR.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form,bool>>>())).Returns(new WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form("name","content",new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType(), new BpsStepPath())
            {
                Id = 1
            });
            moq.Setup(x => x.Forms).Returns(formR.Object);
            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormCommandService service = new FormCommandService(moq.Object);
            service.EditForm(new FormsGenerator.BusinessLogic.Application.DTO.Form
            {
                Name = "test",
                Content = "content",
                Id = 1,
                BpsApplicationGuid = Guid.NewGuid(),
                BpsApplicationName = "name",
                BpsFormTypeName = "name",
                BpsProcessName = "name",
                BpsStartStepPathName = "name",
                BpsWorkflowName = "name",
                BpsWorkflowStartStepName = "name",
                BpsFormTypeGuid = Guid.NewGuid(),
                BpsProcessGuid = Guid.NewGuid(),
                BpsStartStepPathGuid = Guid.NewGuid(),
                BpsWorkflowGuid = Guid.NewGuid(),
                BpsWorkflowStartStepGuid = Guid.NewGuid(),                
            });
        }
        [Test]
        public void ShouldNotFindEditForm()
        {
            var moq = new Moq.Mock<IFormUnitOfWork>();
            var formR = new Moq.Mock<IFormRepository>();
            moq.Setup(x => x.Forms).Returns(formR.Object);

            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormCommandService service = new FormCommandService(moq.Object);
            Assert.Throws<FormNotFoundException>(() => service.EditForm(new FormsGenerator.BusinessLogic.Application.DTO.Form
            {
                Id = 1,
                BpsApplicationGuid = Guid.NewGuid(),
                BpsApplicationName = "name",
                BpsFormTypeName = "name",
                BpsProcessName = "name",
                BpsStartStepPathName = "name",
                BpsWorkflowName = "name",
                BpsWorkflowStartStepName = "name",
                BpsFormTypeGuid = Guid.NewGuid(),
                BpsProcessGuid = Guid.NewGuid(),
                BpsStartStepPathGuid = Guid.NewGuid(),
                BpsWorkflowGuid = Guid.NewGuid(),
                BpsWorkflowStartStepGuid = Guid.NewGuid(),
            }));
        }
        [Test]
        public void ShouldRemoveForm()
        {
            var moq = new Moq.Mock<IFormUnitOfWork>();
            var formR = new Moq.Mock<IFormRepository>();
            moq.Setup(x => x.Forms).Returns(formR.Object);

            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormCommandService service = new FormCommandService(moq.Object);
            service.RemoveForm(1);
        }
        [Test]
        public void ShouldThrowArgumentExceptionOnRemoveForm()
        {
            var moq = new Moq.Mock<IFormUnitOfWork>();
            var formR = new Moq.Mock<IFormRepository>();
            moq.Setup(x => x.Forms).Returns(formR.Object);

            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormCommandService service = new FormCommandService(moq.Object);
            Assert.Throws<ApplicationArgumentException>(() => service.RemoveForm(0));
        }       
    }
}
