using System;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Model
{
    public class Entity
    {
        public int Id { get; set; }
        public Guid Guid { get; protected set; }
    }
}
