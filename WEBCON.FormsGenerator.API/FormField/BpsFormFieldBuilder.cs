using System;
using WEBCON.FormsGenerator.API.FormField;
using WEBCON.FormsGenerator.API.FormField.Value;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.API
{
    public class BpsFormFieldBuilder : IFormFieldBuilder
    {
        public BusinessLogic.Application.DTO.FormField GetFormField(Guid fieldGuid, string name, FieldValue value, bool allowMultipleValues, bool isRequired = false, bool isReadonly = false)
        {
            BusinessLogic.Application.DTO.FormField field = GetField(value);
            if (field == null) return null;
            field.Name = name;
            field.Type = value.Type;
            field.Guid = fieldGuid;
            field.AllowMultipleValues = allowMultipleValues;
            field.IsReadonly = isReadonly;
            field.IsRequired = isRequired;
            return field;
        }
        private BusinessLogic.Application.DTO.FormField GetField(FieldValue fieldValue)
        {
            return fieldValue.Type switch
            {
                FormFieldType.Boolean => new BooleanField(fieldValue),
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
                _ => throw new BpsClientDataException($"Not supported form field type."),
            };
        }
    }
    public class BpsFormFieldValueBuilder : IFormFieldValueBuilder
    {
        public FieldValue GetFormFieldValue(object fieldType, object value)
        {
            return fieldType switch
            {
                "Boolean" => new BpsBooleanValue(value),
                "SingleLine" => new BpsSingleLineValue(value),
                "Decimal" => new BpsDecimalValue(value),
                "Int" => new BpsIntegerValue(value),
                "DateTime" => new BpsDateTimeValue(value),
                "Date" => new BpsDateValue(value),
                "Multiline" => new BpsMultilineValue(value),
                "RatingScale" => new BpsRatingScaleValue(value),
                "SurveyChoose" => new BpsSurveyChooseValue(value),
                "Email" => new BpsEmailValue(value),
                "ChoiceList" => new BpsChoiceListValue(value),
                _ => throw new BpsClientDataException($"{fieldType} is not supported form field type."),
            };
        }
    }
}
