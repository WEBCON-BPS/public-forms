using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.Presentation;
using WEBCON.FormsGenerator.Presentation.Controllers;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Test.Presentation
{
    [TestFixture]
    public class FormControllerTest
    {
        [Test]
        public void CreateShouldReturnViewResult()
        {
            var formService = new Moq.Mock<IFormCommandService>();
            var localizer = new Moq.Mock<IStringLocalizer<FormController>>();
            FormController controller = new FormController(formService.Object,null,null, localizer.Object,null);
            var result = controller.Create();
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            Assert.IsNotNull(view.Model);
            FormViewModel formViewModel = view.Model as FormViewModel;
            Assert.IsNotNull(formViewModel);
        }
        [Test]
        public void CreateShouldReturnEditViewResult()
        {
            var formService = new Moq.Mock<IFormCommandService>();
            formService.Setup(x => x.AddForm(It.IsAny<WEBCON.FormsGenerator.BusinessLogic.Application.DTO.Form>())).Returns(10);
            var localizer = new Moq.Mock<IStringLocalizer<FormController>>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<FormViewModel, Form>(It.IsAny<FormViewModel>())).Returns(new Form());
            FormController controller = new FormController(formService.Object, null, null, localizer.Object, mapperMock.Object);
            var result = controller.Create(new FormViewModel()
            {
                Content = "",
                ContentTransformed = "",
                FrameAncestors = new string[] { },
                IFrame = "",
                IsCaptchaRequired = true,
                ContentFields = null,
                BpsApplicationGuid = Guid.NewGuid(),
                BpsApplicationName = "App",
                BpsFormTypeGuid = Guid.NewGuid(),
                BpsProcessGuid = Guid.NewGuid(),
                BpsWorkflowGuid = Guid.NewGuid(),
                BpsWorkflowStartStepGuid = Guid.NewGuid(),
                Style = "",
                Name = "form"
            });
            var view = result as RedirectToActionResult;
            Assert.IsNotNull(view);
            Assert.IsTrue(view.ActionName.Equals("Edit"));
        }
        [Test]
        public void ShouldDeleteForm()
        {
            var formService = new Moq.Mock<IFormCommandService>();
            var localizer = new Moq.Mock<IStringLocalizer<FormController>>();
            FormController controller = new FormController(formService.Object, null, null, localizer.Object, null);
            var result = controller.Delete(1);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            Assert.IsNotNull(view.Model);
            FormViewModel formViewModel = view.Model as FormViewModel;
            Assert.IsNotNull(formViewModel);
            Assert.AreEqual(1, formViewModel.Id);
        }
        [Test]
        public void ShouldDeleteConfirmForm()
        {
            var formService = new Moq.Mock<IFormCommandService>();
            var localizer = new Moq.Mock<IStringLocalizer<FormController>>();
            FormController controller = new FormController(formService.Object, null, null, localizer.Object, null);
            var result = controller.DeleteConfirmed(1);
            var view = result as RedirectToActionResult;
            Assert.IsNotNull(view);
            Assert.AreEqual("Index",view.ActionName);
            Assert.AreEqual("Forms", view.ControllerName);
        }
    }
}
