using System.Threading.Tasks;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Check connection client definition
    /// </summary>
    public interface IBpsClientCheckConnection
    { 
        /// <summary>
        /// Checks if passed data are correct to connect with BPS
        /// </summary>
        Task<string> CheckConnection(string clientId, string clientSecret, string apiUrl);
    }
}
