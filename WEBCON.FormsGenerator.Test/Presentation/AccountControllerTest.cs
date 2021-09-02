using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using WEBCON.FormsGenerator.Presentation.Controllers;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Test.Presentation
{
    [TestFixture]
    public class AccountControllerTest
    {
        [Test]
        public void ShouldReturnLoginView()
        {
            var configMoq = new Moq.Mock<IConfiguration>();
            var localizer = new Moq.Mock<IStringLocalizer<AccountController>>();
            AccountController controller = new AccountController(configMoq.Object, null, localizer.Object);

            var result = controller.Login();
            var view = result as ViewResult;

            Assert.IsNotNull(view);
            Assert.IsTrue(view.Model is LoginViewModel);
        }
        [Test]
        public async Task ShouldLogin()
        {
            AccountController controller = GetController();
            var result = await controller.Login(new LoginViewModel
            {
                Login = "login",
                Password = "password"
            });
            var view = result as RedirectToActionResult;

            Assert.IsNotNull(view);
            Assert.IsTrue(view.ActionName.Equals("Index"));
        }
        [Test]
        public async Task ShouldReturnInvalidData()
        {
            AccountController controller = GetController();
            var result = await controller.Login(new LoginViewModel
            {
                Login = "log",
                Password = "pass"
            });
            var view = result as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Invalid UserName or Password", controller.ModelState[""].Errors[0].ErrorMessage);
        }
        [Test]
        public async Task ShouldLogout()
        {
            AccountController controller = GetController();
            var result = await controller.Logout();
            var view = result as RedirectToActionResult;

            Assert.IsNotNull(view);
            Assert.IsTrue(view.ControllerName.Equals("Forms") && view.ActionName.Equals("Index"));
        }
        private AccountController GetController()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Login", "login"},
                {"Password", "password"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var asMoq = new Mock<IAuthenticationService>();
            asMoq.Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(asMoq.Object);

            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            var localizer = new Moq.Mock<IStringLocalizer<AccountController>>();
            localizer.Setup(_ => _["Invalid credentials"]).Returns(new LocalizedString("Invalid credentials", "Invalid UserName or Password"));
            return new AccountController(configuration, null, localizer.Object)
            {
                Url = Mock.Of<IUrlHelper>(),
                TempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext()),
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = serviceProviderMock.Object,
                    },
                }
            };
        }
    }
}