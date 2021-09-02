using NUnit.Framework;
using System;
using WEBCON.FormsGenerator.API.ApiModels;
using WEBCON.FormsGenerator.API.FormField;
using WEBCON.FormsGenerator.API.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.Test.API
{
    [TestFixture]
    public class BpsFieldTest
    {
        [Test]
        public void ShouldThrowBooleanValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new BooleanField(new HtmlIntegerValue(1)));       
        }
        [Test]
        public void ShouldThrowIntValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new IntegerField(new HtmlSingleLineValue(1)));
        }
        [Test]
        public void ShouldThrowDecimalValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new DecimalField(new HtmlSingleLineValue(1)));
        }
        [Test]
        public void ShouldThrowSingleLineValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new SingleLineField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldThrowMultilineLineValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new MultilineField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldThrowSurveyChooseValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new SurveyChooseField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldThrowRatingScaleValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new RatingScaleField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldThrowEmailValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new EmailField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldThrowDateValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new DateField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldThrowDateTimeValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new DateTimeField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldThrowChoiceListValueException()
        {
            Assert.Throws<FormFieldValueFormatException>(() => new ChoiceListField(new HtmlIntegerValue(1)));
        }
        [Test]
        public void ShouldReturnValueToBpsForChoiceList()
        {
            ChoiceListField field = new ChoiceListField(new HtmlChoiceListValue("1#Źle"));
            var result = field.ValueToBps() as StartElementValueChoice;
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.id);
            Assert.AreEqual("Źle", result.name);
        }
        [Test]
        public void ShouldReturnValueToBpsForSurveyChooseList()
        {
            SurveyChooseField field = new SurveyChooseField(new HtmlSurveyChooseValue("1#Źle,3#Prawidłowo"));
            var result = field.ValueToBps() as StartElementValueChoices;
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.choices[0].id);
            Assert.AreEqual("Źle", result.choices[0].name);
            Assert.AreEqual("3", result.choices[1].id);
            Assert.AreEqual("Prawidłowo", result.choices[1].name);
        }
        [Test]
        public void ShouldReturnValueToBpsForRatingScale()
        {
            RatingScaleField field = new RatingScaleField(new HtmlRatingScaleValue("10"));
            Assert.AreEqual(10, (int)field.ValueToBps());
        }
        [Test]
        public void ShouldReturnBpsValueDate()
        {
            BpsDateValue value = new BpsDateValue("2020-02-02");
            DateTime date = Convert.ToDateTime(value.Value);
            Assert.AreEqual(2020, date.Year);
            Assert.AreEqual(2, date.Month);
            Assert.AreEqual(2, date.Day);
        }
        [Test]
        public void ShouldNotReturnBpsValueDate()
        {
            Assert.Throws<FormFieldValueConversionException>(() => new BpsDateValue("test"));
            Assert.Throws<FormFieldValueConversionException>(() => new BpsDateTimeValue("test"));
        }
        [Test]
        public void ShouldReturnBpsValueDateTime()
        {
            BpsDateTimeValue value = new BpsDateTimeValue("2020-02-02");
            DateTime date = Convert.ToDateTime(value.Value);
            Assert.AreEqual(2020, date.Year);
            Assert.AreEqual(2, date.Month);
            Assert.AreEqual(2, date.Day);
        }
        [Test]
        public void ShouldReturnBpsEmailTime()
        {
            BpsEmailValue value = new BpsEmailValue("test@gmail.com");
            Assert.AreEqual("test@gmail.com", value.Value);
        }
        [Test]
        public void ShouldNotReturnBpsEmailTime()
        {
            Assert.Throws<FormFieldValueConversionException>(() => new BpsEmailValue("test"));
        }
    }
}
