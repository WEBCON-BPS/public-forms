using System;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Service
{
    public class BpsCheckConnectionService : IBpsCheckConnectionService
    {
        private readonly IBpsClientCheckConnection checkConnection;

        public BpsCheckConnectionService(IBpsClientCheckConnection checkConnection)
        {
            this.checkConnection = checkConnection;
        }
        public async Task<CheckConnectionResult> CheckConnection(string login, string password, string url)
        {
            CheckConnectionResult checkConnectionResult = new CheckConnectionResult();
            try
            {
                var authenticate = await checkConnection.CheckConnection(login, password, url);
                checkConnectionResult.IsConnected = true;
                checkConnectionResult.ResultMessage = "Connected";
            }
            catch (Exception ex)
            {
                checkConnectionResult.IsConnected = false;
                checkConnectionResult.ResultMessage = ex.Message;
            }
            return checkConnectionResult;
        }
    }
}
