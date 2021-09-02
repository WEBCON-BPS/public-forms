using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model.Value;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class HtmlBpsValueTest
    {
        [Test]
        public void ShouldReturnBooleanValue()
        {
            HtmlBooleanValue value = new HtmlBooleanValue("true");
            Assert.AreEqual(true, value.Value);
            value = new HtmlBooleanValue("false");
            Assert.AreEqual(false, value.Value);
            value = new HtmlBooleanValue("on");
            Assert.AreEqual(true, value.Value);
            value = new HtmlBooleanValue("off");
            Assert.AreEqual(false, value.Value);
        }
        [Test]
        public void ShouldChoiceListValue()
        {
            HtmlChoiceListValue value = new HtmlChoiceListValue("1#Option");
            Assert.IsTrue(value.Value is List<ChoiceValue> result && result.First().Id == "1" && result.First().Name == "Option");

            value = new HtmlChoiceListValue("1#Option#1");
            Assert.IsTrue(value.Value is List<ChoiceValue> result2 && result2.First().Id == "1" && result2.First().Name == "Option#1");

            value = new HtmlChoiceListValue("11#Option#1_09");
            Assert.IsTrue(value.Value is List<ChoiceValue> result3 && result3.First().Id == "11" && result3.First().Name == "Option#1_09");

            value = new HtmlChoiceListValue("b#Option#1 test");
            Assert.IsTrue(value.Value is List<ChoiceValue> result4 && result4.First().Id == "b" && result4.First().Name == "Option#1 test");

            value = new HtmlChoiceListValue("125#Option#1");
            Assert.IsTrue(value.Value is List<ChoiceValue> result5 && result5.First().Id == "125" && result5.First().Name == "Option#1");

        }
        [Test]
        public void ShouldReturnDateValue()
        {
            HtmlDateTimeValue value = new HtmlDateTimeValue("2020-02-02");
            Assert.AreEqual(new DateTime(2020,02,02), value.Value);
            Assert.Throws<FormFieldValueConversionException>(() => new HtmlDateTimeValue("false"));       
        }
        [Test]
        public void ShouldReturnDecimalValue()
        {
            HtmlDecimalValue value = new HtmlDecimalValue("3.4");
            Assert.AreEqual(3.4, value.Value);
            Assert.Throws<FormFieldValueConversionException>(() => new HtmlDecimalValue("false"));
        }
        [Test]
        public void ShouldReturnEmailValue()
        {
            HtmlEmailValue value = new HtmlEmailValue("test@mail.com");
            Assert.AreEqual("test@mail.com", value.Value);
            Assert.Throws<FormFieldValueConversionException>(() => new HtmlEmailValue("false"));
        }
        [Test]
        public void ShouldReturnIntegerValue()
        {
            HtmlIntegerValue value = new HtmlIntegerValue("5");
            Assert.AreEqual(5, value.Value);
            Assert.Throws<FormFieldValueConversionException>(() => new HtmlIntegerValue("false"));
        }
        [Test]
        public void ShouldReturnMultilineValue()
        {
            HtmlMultilineValue value = new HtmlMultilineValue("test");
            Assert.AreEqual("test", value.Value);
        }
        [Test]
        public void ShouldReturnSingleLineValue()
        {
            HtmlSingleLineValue value = new HtmlSingleLineValue("test");
            Assert.AreEqual("test", value.Value);
        }
        [Test]
        public void ShouldReturnRatingScaleValue()
        {
            HtmlRatingScaleValue value = new HtmlRatingScaleValue("10");
            Assert.AreEqual(new int[] { 10 }, value.Value);

            value = new HtmlRatingScaleValue("a");
            Assert.AreEqual(null, value.Value);
        }
        [Test]
        public void ShouldReturnSurveyChooseValue()
        {
            HtmlSurveyChooseValue value = new HtmlSurveyChooseValue("1#Good,2#Medium");
            Assert.IsTrue(value.Value is List<ChoiceValue> result && result.First().Id == "1" && result.Last().Name == "Medium");
            
            value = new HtmlSurveyChooseValue("1#Good,2#Medium#2");
            Assert.IsTrue(value.Value is List<ChoiceValue> result2 && result2.First().Id == "1" && result2.Last().Name == "Medium#2");

            value = new HtmlSurveyChooseValue("1#Good,2#Medium#2, 33#Good test_!$");
            Assert.IsTrue(value.Value is List<ChoiceValue> result3 && result3.Last().Id == "33" && result3.Last().Name == "Good test_!$");
        }

    }
}
