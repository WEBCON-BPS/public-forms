using System;
using System.ComponentModel;

namespace WEBCON.FormsGenerator.Presentation.ViewModels
{
    public class FormsViewModel
    {
        [DisplayName("Id")]
        public int FormId { get; set; }
        [DisplayName("Name")]
        public string FormName { get; set; }
        [DisplayName("Created date")]
        public DateTime? Created { get; set; }
        [DisplayName("Application")]
        public string ApplicationName { get; set; }
        [DisplayName("Process")]
        public string ProcessName { get; set; }
        [DisplayName("Workflow")]
        public string WorkflowName { get; set; }
        [DisplayName("Form type")]
        public string FormTypeName { get; set; }
    }
}
