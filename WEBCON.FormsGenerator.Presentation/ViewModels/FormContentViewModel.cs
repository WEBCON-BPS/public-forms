using System.Collections.Generic;

namespace WEBCON.FormsGenerator.Presentation.ViewModels
{
    public class FormContentViewModel
    {
        public List<FormContentFieldViewModel> ContentFields { get; set; }
        public string FormContentWithMetadata { get; set; }
        public string FormContentTransformed { get; set; }
    }
}
