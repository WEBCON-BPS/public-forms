using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class FormQueryServiceTest
    {
        [Test]
        public void ShouldGetForm()
        {
            var moq = UnitOfWorkMoq.Get();
            var formR = new Moq.Mock<IFormRepository>();
            formR.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form, bool>>>())).Returns(new WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form());
            moq.Setup(x => x.Forms).Returns(formR.Object);

            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormQueryService service = new FormQueryService(moq.Object, builderMoq.Object, null);

            service.GetForm(Guid.NewGuid());
        }
        [Test]
        public void ShouldNotFindForm()
        {
            var moq = new Moq.Mock<IFormUnitOfWork>();
            var formR = new Moq.Mock<IFormRepository>();
            formR.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form, bool>>>())).Returns(() => null);
            moq.Setup(x => x.Forms).Returns(formR.Object);

            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormQueryService service = new FormQueryService(moq.Object, builderMoq.Object, null);

            Assert.Throws<FormNotFoundException>(() => service.GetForm(Guid.NewGuid()));
        }
        [Test]
        public void FormGuidNotProvidedForGetForm()
        {
            var moq = new Moq.Mock<IFormUnitOfWork>();
            var builderMoq = new Moq.Mock<IFormBuilder>();
            FormQueryService service = new FormQueryService(moq.Object, builderMoq.Object, null);

            Assert.Throws<ApplicationArgumentException>(() => service.GetForm(0));
            Assert.Throws<ApplicationArgumentException>(() => service.GetForm(Guid.Empty));
        }
    }
}
