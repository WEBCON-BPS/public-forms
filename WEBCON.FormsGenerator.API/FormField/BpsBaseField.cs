using System.Runtime.CompilerServices;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

[assembly: InternalsVisibleTo("WEBCON.FormsGenerator.Test")]
namespace WEBCON.FormsGenerator.API.FormField
{
    /// <summary>
    /// BPS implementation for form field
    /// </summary>
    abstract class BpsBaseField : BusinessLogic.Application.DTO.FormField
    {
        protected BpsBaseField(FieldValue value) : base(value)
        {
        }
        public virtual object ValueToBps()
        {
            return Value.Value;
        }
    }
}
