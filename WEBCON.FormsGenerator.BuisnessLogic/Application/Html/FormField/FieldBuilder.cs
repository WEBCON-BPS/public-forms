using System;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    public class HtmlFormFieldBuilder : IFormFieldBuilder
    {
        public DTO.FormField GetFormField(Guid fieldGuid, string name, FieldValue value, bool allowMultipleValues, bool isRequired = false, bool isReadonly = false)
        {
            DTO.FormField formField = GetField(value);
            formField.Type = value.Type;
            formField.Name = name;
            formField.Guid = fieldGuid;
            formField.AllowMultipleValues = allowMultipleValues;
            formField.IsRequired = isRequired;
            formField.IsReadonly = isReadonly;

            return formField;
        }
        private DTO.FormField GetField(FieldValue fieldValue)
        {
            if (fieldValue == null) return null;
            return fieldValue.Type switch
            {
                FormFieldType.Boolean => new BooleanFormField(fieldValue),
                FormFieldType.SingleLine => new SingleLineField(fieldValue),
                FormFieldType.Multiline => new MultilineField(fieldValue),
                FormFieldType.Date => new DateField(fieldValue),
                FormFieldType.DateTime => new DateTimeField(fieldValue),
                FormFieldType.Decimal => new DecimalField(fieldValue),
                FormFieldType.Integer => new IntegerField(fieldValue),
                FormFieldType.RatingScale => new RatingScaleField(fieldValue),
                FormFieldType.SurveyChoose => new SurveyChooseField(fieldValue),
                FormFieldType.ChoiceList => new ChoiceListField(fieldValue),
                FormFieldType.Email => new EmailField(fieldValue),
                _ => throw new FormFieldValueFormatException($"{fieldValue.Type} is not supported form field type."),
            };
        }
        public class HtmlFormValueBuilder : IFormFieldValueBuilder
        {
            public FieldValue GetFormFieldValue(object fieldType, object value)
            {
                {
                    return fieldType switch
                    {
                        FormFieldType.Boolean => new HtmlBooleanValue(value),
                        FormFieldType.SingleLine => new HtmlSingleLineValue(value),
                        FormFieldType.Decimal => new HtmlDecimalValue(value),
                        FormFieldType.Integer => new HtmlIntegerValue(value),
                        FormFieldType.DateTime => new HtmlDateTimeValue(value),
                        FormFieldType.Date => new HtmlDateValue(value),
                        FormFieldType.Multiline => new HtmlMultilineValue(value),
                        FormFieldType.RatingScale => new HtmlRatingScaleValue(value),
                        FormFieldType.SurveyChoose => new HtmlSurveyChooseValue(value),
                        FormFieldType.Email => new HtmlEmailValue(value),
                        FormFieldType.ChoiceList => new HtmlChoiceListValue(value),
                        _ => throw new FormFieldValueFormatException($"{fieldType} is not supported form field type."),
                    };
                }
            }
        }
    }
}
   
