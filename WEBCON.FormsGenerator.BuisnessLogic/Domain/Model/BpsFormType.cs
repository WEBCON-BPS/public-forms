using System;
using System.Collections.Generic;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsFormType : BpsEntity
    {
        public BpsFormType()
        {
        }

        public BpsFormType(Guid guid, string name) : base(guid, name)
        {          
        }
        public ICollection<Form> Forms { get; set; }
        public ICollection<BpsFormField> FormFields { get; set; }
    }
}
