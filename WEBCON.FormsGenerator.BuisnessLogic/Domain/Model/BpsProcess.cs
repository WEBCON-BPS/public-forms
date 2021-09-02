using System;
using System.Collections.Generic;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsProcess : BpsEntity
    {
        public BpsProcess()
        {

        }
        public BpsProcess(Guid guid, string name, BpsApplication application) : base(guid, name)
        {
            BpsApplication = application ?? throw new BpsEntityArgumentException("Process application is not specified");
        }
        public BpsApplication BpsApplication { get; protected set; }
        public void Update(BpsApplication application)
        {
            BpsApplication = application ?? throw new BpsEntityArgumentException("Process application is not specified");
        }
        public ICollection<BpsWorkflow> BpsWorkflows { get; set; }
    }
}
