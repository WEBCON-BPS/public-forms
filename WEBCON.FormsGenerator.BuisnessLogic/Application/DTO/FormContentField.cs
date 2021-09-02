using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.DTO
{
       public class FormContentField
        {
            public Guid Guid { get; set; }
            public FormField BpsFormField { get; set; }
            public string Name { get; set; }
            public string CustomName { get; set; }
            public bool IsRequired { get; set; }
            public string CustomRequiredWarningText { get; set; }
            public bool AllowMultipleValues { get; set; }
            public bool IsNewField { get; set; }
        }
}
