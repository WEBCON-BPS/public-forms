using System;
using System.Collections.Generic;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsApplication : BpsEntity
    {
        public BpsApplication()
        {

        }
        public BpsApplication(Guid guid, string name) : base(guid, name)
        {            
        }

        public ICollection<BpsProcess> BpsProcesses { get; set; }
    }
}
