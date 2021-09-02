﻿using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Enum;

namespace WEBCON.FormsGenerator.Presentation.ViewModels
{
    public class FormContentFieldViewModel
    {
        public Guid Guid { get; set; }
        public Guid BpsFormFieldGuid { get; set; }
        public string Name { get; set; }
        public string CustomName { get; set; }
        public string BpsName { get; set; }
        public bool IsRequired { get; set; }
        public string CustomRequiredWarningText { get; set; }
        public bool IsNewField { get; set; }
        public FormFieldType Type { get; set; }
        public bool BpsIsReadonly { get; set; }
        public bool BpsIsRequired { get; set; }
        public bool AllowMultipleValues { get; set; }
    }
}
