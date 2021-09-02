using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class BpsEntity : Entity
    {
        public string Name { get; protected set; }
        public BpsEntity()
        {

        }
        public BpsEntity(Guid guid, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new BpsEntityArgumentException("Name cannot be empty");
            if (guid == Guid.Empty)
                throw new ApplicationArgumentException("Model guid is required");
            Guid = guid;
            Name = name;
        }
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new BpsEntityArgumentException("Name cannot be empty");
            Name = name;
        }
    }
}
