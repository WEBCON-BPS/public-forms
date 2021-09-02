using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.Presentation.Controllers;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Test.Presentation
{
    [TestFixture]
    public class BpsCheckConnectionTest
    {
        [Test]
        public async Task ShouldCorrectlyCheckConnection()
        {
            var moq = new Moq.Mock<IBpsClientCheckConnection>();
            moq.Setup(x => x.CheckConnection(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult("token"));

            BpsCheckConnectionService formCheck = new BpsCheckConnectionService(moq.Object);

            BpsCheckConnectionController configuration = new BpsCheckConnectionController(formCheck, null);
            var result = await configuration.CheckConnection("http://test", "login", "pass");
            Assert.IsNotNull(result);
            CheckConnectionResultViewModel checkResult = result.Value as CheckConnectionResultViewModel;
            Assert.IsNotNull(checkResult);
            Assert.AreEqual(true, checkResult.IsConnected);
            Assert.AreEqual("Connected", checkResult.ResultMessage);

        }
        [Test]
        public async Task ShouldThrowUnauthorizedWhenCheckConnection()
        {
            var moq = new Moq.Mock<IBpsClientCheckConnection>();
            moq.Setup(x => x.CheckConnection(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Unauthorized"));

            BpsCheckConnectionService formCheck = new BpsCheckConnectionService(moq.Object);

            BpsCheckConnectionController configuration = new BpsCheckConnectionController(formCheck, null);
            var result = await configuration.CheckConnection("http://test", "login", "pass");
            Assert.IsNotNull(result);
            CheckConnectionResultViewModel checkResult = result.Value as CheckConnectionResultViewModel;
            Assert.IsNotNull(checkResult);
            Assert.AreEqual(false, checkResult.IsConnected);
            Assert.AreEqual("Unauthorized", checkResult.ResultMessage);
        }
        [Test]
        public async Task ShouldThrowLoginDataNotPassedWhenCheckConnection()
        {
            var moq = new Moq.Mock<IBpsClientCheckConnection>();
            BpsCheckConnectionService formCheck = new BpsCheckConnectionService(moq.Object);
            BpsCheckConnectionController configuration = new BpsCheckConnectionController(formCheck, null);
            var result = await configuration.CheckConnection("", "", "");
            Assert.IsNotNull(result);
            CheckConnectionResultViewModel checkResult = result.Value as CheckConnectionResultViewModel;
            Assert.IsNotNull(checkResult);
            Assert.AreEqual(false, checkResult.IsConnected);
            Assert.AreEqual("Data for login not provided", checkResult.ResultMessage);
        }
    }
}
