using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.API;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using static WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.HtmlFormFieldBuilder;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class BpsStartElementServiceTest
    {
        [Test]
        public async Task ShouldStartElement()
        {
            var clientService = new Moq.Mock<IBpsClientCommandService>();
            var dbMoq = UnitOfWorkMoq.Get();
            var formR = new Moq.Mock<IFormRepository>();

            var fieldsR = new Moq.Mock<IRepository<FormsGenerator.BusinessLogic.Domain.Model.BpsFormField>>();
            Guid dateGuid = Guid.NewGuid();
            Guid boolean1Guid = Guid.NewGuid();
            Guid boolean2Guid = Guid.NewGuid();
            Guid decimalGuid = Guid.NewGuid();
            Guid integerGuid = Guid.NewGuid();
            Guid surveyGuid = Guid.NewGuid();
            Guid choiceGuid = Guid.NewGuid();
            Guid ratingGuid = Guid.NewGuid();
            Guid emailGuid = Guid.NewGuid();
            Guid singlelineGuid = Guid.NewGuid();
            Guid multilineGuid = Guid.NewGuid();
            fieldsR.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.BpsFormField, bool>>>())).Returns(new List<FormsGenerator.BusinessLogic.Domain.Model.BpsFormField>
            {
                new BpsFormField(dateGuid,"field", FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.Date, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(boolean1Guid,"field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.Boolean, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(boolean2Guid,"field", FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.Boolean, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(decimalGuid,"field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.Decimal, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(integerGuid,"field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.Integer, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(singlelineGuid,"field", FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.SingleLine, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(multilineGuid,"field", FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.Multiline, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(emailGuid,"field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.Email, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(ratingGuid,"field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.RatingScale, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(surveyGuid,"field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.SingleLine, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
                new BpsFormField(choiceGuid,"field",  FormsGenerator.BusinessLogic.Domain.Enum.FormFieldType.ChoiceList, new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()),
             });
            formR.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form, bool>>>())).Returns(
                new WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form("test", "test", new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType(Guid.NewGuid(), "form"), new BpsStepPath())
                {
                    Id = 12
                });
            dbMoq.Setup(x => x.BpsFormFields).Returns(fieldsR.Object);
            dbMoq.Setup(x => x.Forms).Returns(formR.Object);
            var encoding = new Moq.Mock<IDataEncoding>();

            clientService.Setup(x => x.StartElement(It.IsAny<IEnumerable<FormsGenerator.BusinessLogic.Application.DTO.FormField>>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), null))
                .Returns(Task.FromResult(new StartElementResult { Id = 1212, Number = "P/124", Status = "MovedToTheNextStep" }));
            BpsStartElementService bpsFormService = new BpsStartElementService(clientService.Object, dbMoq.Object, encoding.Object, new FormFieldFactory(new BpsFormFieldBuilder(),new HtmlFormValueBuilder()));
            var result = await bpsFormService.Start(new List<KeyValuePair<Guid, object>>
            { new KeyValuePair<Guid,object>(dateGuid,"2020-05-05") ,
             new KeyValuePair<Guid, object>(boolean1Guid, "on"),
             new KeyValuePair<Guid, object>(boolean2Guid, "off"),
             new KeyValuePair<Guid, object> (decimalGuid, 1.05),
             new KeyValuePair<Guid, object>(integerGuid, 10),
             new KeyValuePair<Guid, object>(singlelineGuid,"test"),
             new KeyValuePair<Guid, object>(multilineGuid,"test"),
             new KeyValuePair<Guid, object>(emailGuid,"test@gmail.com"),
             new KeyValuePair<Guid, object>(ratingGuid, 1),
             new KeyValuePair<Guid, object>(surveyGuid, "a#Correct,b#Wrong"),
             new KeyValuePair<Guid, object>(choiceGuid, "a#Correct") }, Guid.NewGuid());
            Assert.IsTrue(result.Id == 1212 && result.Number == "P/124");
        }
        [Test]
        public void ShouldNotStartElementBecauseOfNotInitializedDb()
        {
            var clientService = new Moq.Mock<IBpsClientCommandService>();
            var encoding = new Moq.Mock<IDataEncoding>();
            Assert.Throws<ApplicationArgumentException>(() => { new BpsStartElementService(clientService.Object, null, encoding.Object, new FormFieldFactory(null,null)); }, "Data source instance has not been initialized.");
        }
        [Test]
        public void ShouldNotStartElementBecauseFieldsNotProvided()
        {
            var clientService = new Moq.Mock<IBpsClientCommandService>();     
            var encoding = new Moq.Mock<IDataEncoding>();
            var dbMoq = new Moq.Mock<IFormUnitOfWork>();
            BpsStartElementService bpsFormService = new BpsStartElementService(clientService.Object, dbMoq.Object, encoding.Object, new FormFieldFactory(null,null));
            Assert.Throws<ApplicationArgumentException>(() => bpsFormService.Start(null, Guid.NewGuid()), "Form fields has not been provided. Could not start element.");
        }
        [Test]
        public void ShouldNotStartElementBecauseFormGuidNotProvided()
        {
            var clientService = new Moq.Mock<IBpsClientCommandService>();
            var encoding = new Moq.Mock<IDataEncoding>();
            var dbMoq = new Moq.Mock<IFormUnitOfWork>();
            BpsStartElementService bpsFormService = new BpsStartElementService(clientService.Object, dbMoq.Object, encoding.Object, new FormFieldFactory(null,null));
            Assert.Throws<ApplicationArgumentException>(() => bpsFormService.Start(new List<KeyValuePair<Guid, object>>{
                new KeyValuePair<Guid, object>(Guid.NewGuid(), "2020-05-05") }, Guid.Empty), "Form guid has not been provided.");
        }
        [Test]
        public void ShouldNotStartElementBecauseFormNotFound()
        {
            var clientService = new Moq.Mock<IBpsClientCommandService>();
            var encoding = new Moq.Mock<IDataEncoding>();
            var dbMoq = new Moq.Mock<IFormUnitOfWork>();
            var formR = new Moq.Mock<IFormRepository>();
            dbMoq.Setup(x => x.Forms).Returns(formR.Object);
            BpsStartElementService bpsFormService = new BpsStartElementService(clientService.Object, dbMoq.Object, encoding.Object, new FormFieldFactory(null,null));
            Assert.Throws<FormNotFoundException>(() => bpsFormService.Start(new List<KeyValuePair<Guid, object>>{
                new KeyValuePair<Guid, object>(Guid.NewGuid(), "2020-05-05") }, Guid.NewGuid()), "Could not find form with specified guid.");
        }
        [Test]
        public void ShouldNotStartBecauseEncodingIsNotProvided()
        {
            var clientService = new Moq.Mock<IBpsClientCommandService>();
            var dbMoq = new Moq.Mock<IFormUnitOfWork>();
            var formR = new Moq.Mock<IFormRepository>();
            var fieldGuid = Guid.NewGuid();
            var fieldsR = new Moq.Mock<IRepository<FormsGenerator.BusinessLogic.Domain.Model.BpsFormField>>();
           // fieldsR.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.BpsFormField, bool>>>())).Returns(new List<BpsFormField> { new BpsFormField(fieldGuid, "Test", new HtmlDateTimeValue("2020-05-05"), new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType()) });
            var moqForm = new WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form("test", "test", new FormsGenerator.BusinessLogic.Domain.Model.BpsFormType(), new BpsStepPath())
            {
                Id = 12
            };
            moqForm.SetCustomCredentials("clientId", "clientSecret");
            formR.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Form, bool>>>())).Returns(moqForm);
            dbMoq.Setup(x => x.BpsFormFields).Returns(fieldsR.Object);
            dbMoq.Setup(x => x.Forms).Returns(formR.Object);
            BpsStartElementService bpsFormService = new BpsStartElementService(clientService.Object, dbMoq.Object, null, new FormFieldFactory(new BpsFormFieldBuilder(),new HtmlFormValueBuilder()));
            Assert.Throws<ApplicationArgumentException>(() => bpsFormService.Start(new List<KeyValuePair<Guid, object>>{
                new KeyValuePair<Guid, object>(fieldGuid, "2020-05-05") }, Guid.NewGuid()), "Could not start element because could not retrive user credendials."); ;
        }
    }
}
