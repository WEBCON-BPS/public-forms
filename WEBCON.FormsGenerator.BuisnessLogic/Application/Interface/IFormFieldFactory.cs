using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Definition for creating form field from BPS implementation and to BPS implementation
    /// </summary>
    public interface IFormFieldFactory
    {
        IFormFieldBuilder FormFieldBuilder { get;  }
        IFormFieldValueBuilder FormValueBuilder { get;  }
    }
    public interface IFormFieldBuilder
    {
        DTO.FormField GetFormField(Guid fieldGuid, string name, FieldValue value, bool allowMultipleValues, bool isRequired = false, bool isReadonly = false);
    }
    public interface IFormFieldValueBuilder
    {
        FieldValue GetFormFieldValue(object fieldType, object value);
    }

}
