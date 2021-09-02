using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;

namespace WEBCON.FormsGenerator.API
{
    public class BpsApiClientCheckConnection : ApiBaseClient, IBpsClientCheckConnection
    {
        public async Task<string> CheckConnection(string clientId, string clientSecret, string apiUrl)
        {
            base.ApiUrl = apiUrl;
            return await AuthenticateWithCustomCredentials(new Credentials(clientId, clientSecret));
        }
    }
}
