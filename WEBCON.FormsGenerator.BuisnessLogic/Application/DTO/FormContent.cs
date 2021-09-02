using System.Collections.Generic;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.DTO
{
    public class FormContent
    {
        public List<FormContentField> ContentFields { get; set; }
        public string FormContentWithMetadata { get; set; }
        public string FormContentTransformed { get; set; }
    }    
}
