using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using WEBCON.FormsGenerator.API;
using WEBCON.FormsGenerator.API.ApiModels;
using WEBCON.FormsGenerator.API.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Helper;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class HtmlFormBuilderTest
    {
        [Test]
        public void ShouldCreateLabelTest()
        {
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string createdLabel = htmlFormBuilder.CreateLabel("labelTest", "labelValue");
            Assert.AreEqual(@"<label id=""labelTest"">labelValue</label>", createdLabel);
            createdLabel = htmlFormBuilder.CreateLabel("labelTest", "{labelTest}");
            Assert.AreEqual(@"<label id=""labelTest"">{labelTest}</label>", createdLabel);
            createdLabel = htmlFormBuilder.CreateLabel(null, null);
            Assert.AreEqual(@"<label id=""""></label>", createdLabel);
        }
        [Test]
        public void ShouldCreateSingleLineInputField()
        {
            FormFieldFactory formFieldFactory = new FormFieldFactory(new HtmlFormFieldBuilder(),new BpsFormFieldValueBuilder());
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string field = htmlFormBuilder.CreateFormFieldBasedOnBpsField(formFieldFactory.FormFieldBuilder.GetFormField(new Guid(), "isFieldRequired", new HtmlSingleLineValue("test") , false, false));
            Assert.AreEqual(@$"<input type=""text"" data-bpsname=""isFieldRequired"" id=""{new Guid()}"" name=""{new Guid()}"" value=""test"""+" {isRequired} {requiredWarningText}>", field);
        }
        [Test]
        public void ShouldCreateBooleanInputField()
        {
            FormFieldFactory formFieldFactory = new FormFieldFactory(new HtmlFormFieldBuilder(), new BpsFormFieldValueBuilder());
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string field = htmlFormBuilder.CreateFormFieldBasedOnBpsField(formFieldFactory.FormFieldBuilder.GetFormField(new Guid(), "temperature", new HtmlBooleanValue(""), false, false));
            Assert.AreEqual(@$"<input type=""checkbox"" data-bpsname=""temperature"" id=""{new Guid()}"" name=""{new Guid()}"""+" {isRequired} {requiredWarningText}>", field);         
        }
        [Test]
        public void ShouldCreateDecimalInputField()
        {
            FormFieldFactory formFieldFactory = new FormFieldFactory(new HtmlFormFieldBuilder(), new BpsFormFieldValueBuilder());
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string field = htmlFormBuilder.CreateFormFieldBasedOnBpsField(formFieldFactory.FormFieldBuilder.GetFormField(new Guid(), "quantity", new HtmlDecimalValue(1.5), false, false));
            Assert.AreEqual(@$"<input type=""number"" step="".01"" id=""{new Guid()}"" data-bpsname=""quantity"" name=""{new Guid()}"" value=""1,5""" + " {isRequired} {requiredWarningText}>", field);
        }
        [Test]
        public void ShouldCreateIntegerInputField()
        {
            FormFieldFactory formFieldFactory = new FormFieldFactory(new HtmlFormFieldBuilder(), new BpsFormFieldValueBuilder());
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string field = htmlFormBuilder.CreateFormFieldBasedOnBpsField(formFieldFactory.FormFieldBuilder.GetFormField(new Guid(), "age", new HtmlIntegerValue(1), false, false));
            Assert.AreEqual(@$"<input type=""number"" data-bpsname=""age"" id=""{new Guid()}"" name=""{new Guid()}"" value=""1"""+" {isRequired} {requiredWarningText}>", field);
        }
        [Test]
        public void ShouldCreateDateInputField()
        {
            FormFieldFactory formFieldFactory = new FormFieldFactory(new HtmlFormFieldBuilder(), new BpsFormFieldValueBuilder());
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string field = htmlFormBuilder.CreateFormFieldBasedOnBpsField(formFieldFactory.FormFieldBuilder.GetFormField(new Guid(), "created", new HtmlDateValue(null), false, false));
            Assert.AreEqual(@$"<input type=""text"" data-bpsname=""created"" date-control=""true"" id=""{new Guid()}"" name=""{new Guid()}"" value=""""" + " {isRequired} {requiredWarningText}>", field);
        }
        [Test]
        public void ShouldntCreateInputField()
        {
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            Assert.Throws<ApplicationArgumentException>(() => htmlFormBuilder.CreateFormFieldBasedOnBpsField(null));         
        }
        [Test]
        public void ShouldCreateEmptyInputField()
        {
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string field = htmlFormBuilder.CreateFormFieldBasedOnBpsField(new FormsGenerator.BusinessLogic.Application.DTO.FormField()
            { Guid = new Guid(), Name = "temperature"});
            Assert.AreEqual("", field);
        }
        [Test]
        public void ShouldCreateFormForInputs()
        {
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<input type=""text"" id=""1221"" name=""Field1"">");
            stringBuilder.Append(@"<input type=""checkbox"" id=""2523"" name=""Field2"">");
            stringBuilder.Append(@"<input type=""number"" id=""3251"" name=""Field2"">");
            string transformedFields = htmlFormBuilder.EmbedFieldsInForm(stringBuilder.ToString());
            Assert.AreEqual($@"<form id=""generated-form""><input type=""text"" id=""1221"" name=""Field1""><input type=""checkbox"" id=""2523"" name=""Field2""><input type=""number"" id=""3251"" name=""Field2""><input type=""submit"" value=""{FormParameters.ContentSubmitText}""></form>", transformedFields.Replace("\r\n",""));
        }
        [Test]
        public void ShouldTransformMetadataOnForm()
        {
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            List<FormContentField> metadatas = new List<FormContentField>
            {
                 new FormContentField{ Name = "label_field", CustomName = "label_name"}
            };
            string formWithMetadata = @"<form action=Form/StartElement><label id=""label_field"">{label_field}</label><input type=""text"" id=""1221"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            string transformed = htmlFormBuilder.TransformMetadataOnForm(metadatas, formWithMetadata,"");
            Assert.AreEqual(@"<form action=Form/StartElement><label id=""label_field"">label_name</label><input type=""text"" id=""1221"" name=""Field1""><input type=""submit"" value=""Submit""></form>".Trim(), transformed.Trim());

        }
        [Test]
        public void TransformNullMetadataOnForm()
        {
            HtmlFormBuilder htmlFormBuilder = new HtmlFormBuilder();
            string formWithMetadata = @"<form action=Form/StartElement><label id=""label_field"">{label_field}</label><input type=""text"" id=""1221"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            string transformed = htmlFormBuilder.TransformMetadataOnForm(null, formWithMetadata, "");
            Assert.AreEqual(formWithMetadata.Trim(), transformed.Trim());

        }
        [Test]
        public void ShouldCreateFormNameArea()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Assert.AreEqual("<h4>form name</h4>", builder.CreateFormNameArea("form name"));
        }
        [Test]
        public void ShouldCreateFormDescriptionArea()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Assert.AreEqual("<p>form description</p>", builder.CreateFormDescriptionArea("form description"));
        }
        [Test]
        public void ShouldEmbedControlsInGroup()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            var result = builder.EmbedInputsInGroup(new string[] { "test control 1", "test control 2" }, "test");
            Assert.AreEqual(@"<div id=""group-test"">test control 1test control 2</div>", result.Replace("\r\n",""));
        }
        [Test]
        public void ShouldNotEmbedEmptyControlsInGroup()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            var result = builder.EmbedInputsInGroup(null,null);
            Assert.AreEqual("", result);
        }
        [Test]
        public void ShouldEmbedInputsInExistingForm()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            string existingContent = @"<form action=Form/StartElement><label id=""label_field"">{label_field}</label><input type=""text"" id=""1221"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            string[] inputs = new string[]
            {
                 @"<input type=""text"" id=""1821"" name=""Field1"">",
                 @"<input type=""checkbox"" id=""1822"" name=""Field1"">",
            };
            var result = builder.EmbedInputsInExistingForm(inputs, existingContent);
            Assert.AreEqual(@"<form action=Form/StartElement><label id=""label_field"">{label_field}</label><input type=""text"" id=""1221"" name=""Field1""><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>", result.Replace("\r\n",null));
        }
        [Test]
        public void ShouldRemoveInputsFromExistingForm()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid guid = Guid.NewGuid();
            string existingContent = @"<form action=""Form/StartElement""><label id=""field_"+guid+@""">{label_field}</label><input type=""text"" id="""+guid+@""" name=""Field1""><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            var result = builder.RemoveFieldFromExistingForm(guid, existingContent);
            Assert.AreEqual(@"<form action=""Form/StartElement""><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>", result.Replace("\r\n", null));
        }
        [Test]
        public void ShouldRemoveInputsFromExistingFormWhenFieldsAreInDiv()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid guid = Guid.NewGuid();
            string existingContent = @"<form action=""Form/StartElement""><div><label id=""field_"+guid+@""">{label_field}</label><input type=""text"" id=""" + guid + @""" name=""Field1""></div><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            var result = builder.RemoveFieldFromExistingForm(guid, existingContent);
            Assert.AreEqual(@"<form action=""Form/StartElement""><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>", result.Replace("\r\n", null));
        }
        [Test]
        public void ShouldRemoveInputsFromExistingFormWhenFieldsAreInGroupDiv()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid guid = Guid.NewGuid();
            string existingContent = @"<form action=""Form/StartElement""><div id=""group-"+guid+@"""><label id=""field_" + guid + @""">{label_field}</label><input type=""text"" id=""" + guid + @""" name=""Field1""></div><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            var result = builder.RemoveFieldFromExistingForm(guid, existingContent);
            Assert.AreEqual(@"<form action=""Form/StartElement""><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>", result.Replace("\r\n", null));
        }
        [Test]
        public void ShouldRemoveSelectFromExistingFormWhenFieldsAreInGroupDiv()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid guid = Guid.NewGuid();
            string existingContent = @"<form action=""Form/StartElement""><div id=""group-"+guid+@"""><select><option id=""guid"">M</option></select></div><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            var result = builder.RemoveFieldFromExistingForm(guid, existingContent);
            Assert.AreEqual(@"<form action=""Form/StartElement""><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>", result.Replace("\r\n", null));
        }
        [Test]
        public void ShouldRemoveSelectFromExistingFormWhenFieldsAreInDiv()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid guid = Guid.NewGuid();
            string existingContent = @"<form action=""Form/StartElement""><div><select id="""+guid+@"""><option id="""+guid+@""">M</option></select></div><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>";
            var result = builder.RemoveFieldFromExistingForm(guid, existingContent);
            Assert.AreEqual(@"<form action=""Form/StartElement""><input type=""text"" id=""1821"" name=""Field1""><input type=""checkbox"" id=""1822"" name=""Field1""><input type=""submit"" value=""Submit""></form>", result.Replace("\r\n", null));
        }
        [Test]
        public void ShouldReturnIntInput()
        {
            IntegerField field = new IntegerField(new BpsIntegerValue(1));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<input type=""number"" data-bpsname="""" id=""{Guid.Empty}"" name=""{Guid.Empty}"" value=""1"" {FormParameters.IsRequired} {FormParameters.RequiredText}>", input);
        }
        [Test]
        public void ShouldReturnDecimalInput()
        {
            DecimalField field = new DecimalField(new BpsDecimalValue(1.05));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<input type=""number"" step="".01"" id=""{Guid.Empty}"" data-bpsname="""" name=""{Guid.Empty}"" value=""1,05"" {FormParameters.IsRequired} {FormParameters.RequiredText}>", input);
        }
        [Test]
        public void ShouldReturnEmailInput()
        {
            EmailField field = new EmailField(new BpsEmailValue(null));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<input type=""email"" data-bpsname="""" id=""{Guid.Empty}"" name=""{Guid.Empty}"" value="""" {FormParameters.IsRequired} {FormParameters.RequiredText}>", input);
        }
        [Test]
        public void ShouldReturnSingleLineInput()
        {
            SingleLineField field = new SingleLineField(new BpsSingleLineValue("test"));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<input type=""text"" data-bpsname="""" id=""{Guid.Empty}"" name=""{Guid.Empty}"" value=""test"" {FormParameters.IsRequired} {FormParameters.RequiredText}>", input);
        }
        [Test]
        public void ShouldReturnMultilineInput()
        {
            MultilineField field = new MultilineField(new BpsMultilineValue(null));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<textarea id=""{Guid.Empty}"" data-bpsname="""" name=""{Guid.Empty}"" value="""" {FormParameters.IsRequired} {FormParameters.RequiredText}></textarea>", input);
        }
        [Test]
        public void ShouldReturnCheckboxInput()
        {
            BooleanFormField field = new BooleanFormField(new BpsBooleanValue(null));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<input type=""checkbox"" data-bpsname="""" id=""{Guid.Empty}"" name=""{Guid.Empty}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>", input);
        }
        [Test]
        public void ShouldReturnCheckboxCheckedInput()
        {
            BooleanFormField field = new BooleanFormField(new BpsBooleanValue("true"));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<input type=""checkbox"" data-bpsname="""" id=""{Guid.Empty}"" name=""{Guid.Empty}"" checked {FormParameters.IsRequired} {FormParameters.RequiredText}>", input);
        }
        [Test]
        public void ShouldReturnRatingScaleInput()
        {
            RatingScaleField field = new RatingScaleField(new BpsRatingScaleValue(new FormLayoutScaleValues { min=1, max=1 }));
            string input = field.CreateInput();
            Assert.AreEqual(@$"<div><input type=""radio"" id=""{Guid.Empty}"" data-bpsname="""" name=""{Guid.Empty}"" value=""1"" checked {FormParameters.IsRequired} {FormParameters.RequiredText} ><label>1</label></div>", input.Replace("\r\n",null));
        }
        [Test]
        public void ShouldReturnChoiceListInput()
        {
            ChoiceListField field = new ChoiceListField(new BpsChoiceListValue(new List<FormFieldValueData> {
                new FormFieldValueData { id = "a", name="Wrong" },
                new FormFieldValueData { id = "b", name="Medium" },
                new FormFieldValueData { id = "c", name="Good"}
            }.ToArray()));
            string input = field.CreateInput();
            string expected = @$"<select data-bpsname="""" name=""{Guid.Empty}"" id=""{Guid.Empty}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
            <option id =""{Guid.Empty}"" value=""a#Wrong"">Wrong</option>
            <option id =""{Guid.Empty}"" value=""b#Medium"">Medium</option>
            <option id =""{Guid.Empty}"" value=""c#Good"">Good</option>
            </select>".Replace("\r\n", null).Replace(" ", null);
            Assert.AreEqual(expected, input.Replace("\r\n", null).Replace(" ", null));
        }
        [Test]
        public void ShouldReturnSurveyChooseInput()
        {
            SurveyChooseField field = new SurveyChooseField(new BpsSurveyChooseValue(new List<FormFieldValueData> {
                new FormFieldValueData { id = "a", name="Wrong" },
                new FormFieldValueData { id = "b", name="Medium" },
                new FormFieldValueData { id = "c", name="Good"}
            }.ToArray()));
            string input = field.CreateInput();
            string expected = @$"<fieldset id=""{Guid.Empty}"" data-bpsname="""" {FormParameters.AllowMultipleValues} name=""{Guid.Empty}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
            <div><input type=""checkbox"" id=""{Guid.Empty}"" name=""{Guid.Empty}"" value=""a#Wrong"" ><label>Wrong</label></div>
            <div><input type=""checkbox"" id=""{Guid.Empty}"" name=""{Guid.Empty}"" value=""b#Medium"" ><label>Medium</label></div>
            <div><input type=""checkbox"" id=""{Guid.Empty}"" name=""{Guid.Empty}"" value=""c#Good"" ><label>Good</label></div>        
            </fieldset>".Replace("\r\n",null).Replace(" ",null);
            Assert.AreEqual(expected, input.Replace("\r\n", null).Replace(" ", null));
        }
        [Test]
        public void ShouldUpdateValuesInChoiceListWithoutDivGroup()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid selectGuid = Guid.NewGuid();
            string existingContent = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<select data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<option id =""{selectGuid}"" value=""a#High"">High</option>
<option id =""{selectGuid}"" value=""b#Low"">Low</option>
<option id =""{selectGuid}"" value=""c#Higher"">Higher</option>
</select>
<input type=""submit"" value=""Submit""></form>";

            var result = builder.UpdateChoices(new ChoiceListField(new BpsChoiceListValue(new FormFieldValueData[]
                {
                     new FormFieldValueData { id = "a", name = "High"},
                     new FormFieldValueData { id = "b", name = "Low"},
                     new FormFieldValueData { id = "c", name = "Medium"  }
                }))
            { Guid = selectGuid }, existingContent);
            string expected = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<select data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<option id=""{selectGuid}"" value=""a#High"">High</option>
<option id=""{selectGuid}"" value=""b#Low"">Low</option>
<option id=""{selectGuid}"" value=""c#Medium"">Medium</option>
</select>
<input type=""submit"" value=""Submit""></form>";
            Assert.AreEqual(expected.Replace("\r\n", ""), result.Replace("\r\n", ""));
        }
        [Test]
        public void ShouldUpdateValuesInChoiceListWithDivGroup()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid selectGuid = Guid.NewGuid();
            string existingContent = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<div id=""group-{selectGuid}""><select data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<option id =""{selectGuid}"" value=""a#High"">High</option>
<option id =""{selectGuid}"" value=""b#Low"">Low</option>
<option id =""{selectGuid}"" value=""c#Higher"">Higher</option>
</select>
</div>
<input type=""submit"" value=""Submit""></form>";

            var result = builder.UpdateChoices(new ChoiceListField(new BpsChoiceListValue(new FormFieldValueData[]
                {
                     new FormFieldValueData { id = "a", name = "High"},
                     new FormFieldValueData { id = "b", name = "Low"},
                     new FormFieldValueData { id = "c", name = "Medium"  }
                }))
            { Guid = selectGuid }, existingContent);
            string expected = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<div id=""group-{selectGuid}""><select data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<option id=""{selectGuid}"" value=""a#High"">High</option>
<option id=""{selectGuid}"" value=""b#Low"">Low</option>
<option id=""{selectGuid}"" value=""c#Medium"">Medium</option>
</select>
</div>
<input type=""submit"" value=""Submit""></form>";
            Assert.AreEqual(expected.Replace("\r\n", ""), result.Replace("\r\n", ""));
        }
        [Test]
        public void ShouldUpdateValuesInSurveyChoosetWithoutDivGroup()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid selectGuid = Guid.NewGuid();
            string existingContent = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<fieldset data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""a#High""><label>High</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""b#Low""><label>Low</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""c#Higher""><label>Higher</label></div>
</fieldset>
<input type=""submit"" value=""Submit""></form>";

            var result = builder.UpdateChoices(new SurveyChooseField(new BpsSurveyChooseValue(new FormFieldValueData[]
                {
                     new FormFieldValueData { id = "a", name = "High"},
                     new FormFieldValueData { id = "b", name = "Low"},
                     new FormFieldValueData { id = "c", name = "Medium"  }
                }))
            { Guid = selectGuid}, existingContent);
            string expected = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<fieldset data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""a#High""><label>High</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""b#Low""><label>Low</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""c#Medium""><label>Medium</label></div>
</fieldset>
<input type=""submit"" value=""Submit""></form>";
            Assert.AreEqual(expected.Replace("\r\n", ""), result.Replace("\r\n", ""));
        }
        [Test]
        public void ShouldUpdateValuesInSurveyChoosetWithDivGroup()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid selectGuid = Guid.NewGuid();
            string existingContent = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<div id=""group-{selectGuid}""><fieldset data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""a#High""><label>High</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""b#Low""><label>Low</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""c#Higher""><label>Higher</label></div>
</fieldset>
</div>
<input type=""submit"" value=""Submit""></form>";

            var result = builder.UpdateChoices(new SurveyChooseField(new BpsSurveyChooseValue(new FormFieldValueData[]
                {
                     new FormFieldValueData { id = "a", name = "High"},
                     new FormFieldValueData { id = "b", name = "Low"},
                     new FormFieldValueData { id = "c", name = "Medium"  }
                }))
            { Guid = selectGuid }, existingContent);
            string expected = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<div id=""group-{selectGuid}""><fieldset data-bpsname=""test"" name=""{selectGuid}"" id=""{selectGuid}"" {FormParameters.IsRequired} {FormParameters.RequiredText}>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""a#High""><label>High</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""b#Low""><label>Low</label></div>
<div><input type=""checkbox"" id=""{selectGuid}"" name=""{selectGuid}"" value=""c#Medium""><label>Medium</label></div>
</fieldset>
</div>
<input type=""submit"" value=""Submit""></form>";
            Assert.AreEqual(expected.Replace("\r\n", ""), result.Replace("\r\n", ""));
        }
        [Test]
        public void ShouldUpdateValuesInRatingScaletWithDivGroup()
        {
            HtmlFormBuilder builder = new HtmlFormBuilder();
            Guid selectGuid = Guid.NewGuid();
            string existingContent = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<div id=""group-{selectGuid}"">
<div><input type=""radio""id = ""{selectGuid}"" data-bpsname=""test"" name=""{selectGuid}"" value = ""1"" checked { FormParameters.IsRequired} { FormParameters.RequiredText}>
<label>1</label></div>
<div><input type=""radio"" id= ""{selectGuid}"" data-bpsname=""test"" name=""{selectGuid}"" value= ""2"" { FormParameters.IsRequired} { FormParameters.RequiredText}>
<label>2</label></div>
<div><input type=""radio"" id=""{selectGuid}"" data-bpsname=""test"" name=""{selectGuid}"" value= ""4"" { FormParameters.IsRequired} { FormParameters.RequiredText}>
<label>4</label></div>
</div>
<input type=""submit"" value=""Submit""></form>";

            var result = builder.UpdateChoices(new RatingScaleField(new BpsRatingScaleValue(new FormLayoutScaleValues
            {
                min = 1,
                max = 3
            }))
            { Guid = selectGuid, Name = "test" }, existingContent);
            string expected = @$"<form><div><label id=""label_field"">label</label><input type=""text"" id=""1221"" name=""Field1""></div>
<div id=""group-{selectGuid}"">
<div><input type=""radio"" id=""{selectGuid}"" data-bpsname=""test"" name=""{selectGuid}"" value=""1"" checked="""" { FormParameters.IsRequired} { FormParameters.RequiredText}>
<label>1</label></div>
<div><input type=""radio"" id=""{selectGuid}"" data-bpsname=""test"" name=""{selectGuid}"" value=""2"" { FormParameters.IsRequired} { FormParameters.RequiredText}>
<label>2</label></div>
<div><input type=""radio"" id=""{selectGuid}"" data-bpsname=""test"" name=""{selectGuid}"" value=""3"" { FormParameters.IsRequired} { FormParameters.RequiredText}>
<label>3</label></div>
</div>
<input type=""submit"" value=""Submit""></form>";
            Assert.AreEqual(expected.Replace("\r\n", ""), result.Replace("\r\n", ""));
        }
    }
}
