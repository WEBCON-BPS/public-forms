using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    public interface IBpsCheckConnectionService
    {
        /// <summary>
        /// Checks if passed data are correct to connect with BPS
        /// </summary>
        Task<CheckConnectionResult> CheckConnection(string login, string password, string url); 
    }
}
