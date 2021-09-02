using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsStepPath : BpsEntity
    {
        public BpsStepPath()
        {

        }
        public BpsStepPath(Guid guid, string name, BpsWorkflowStep step) : base(guid, name)
        {
            BpsWorkflowStep = step ?? throw new BpsEntityArgumentException("Path step is not specified");
        }
        public BpsWorkflowStep BpsWorkflowStep { get; set; }
        public void Update(BpsWorkflowStep step)
        {
            BpsWorkflowStep = step ?? throw new BpsEntityArgumentException("Path step is not specified");
        }
    }
}
