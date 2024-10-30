using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.API.ApiModels;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.API
{
    public class ApiBaseClient
    {
        protected string ApiUrl;      
        private string _currentToken = null;
        private readonly Credentials _credentials;

        public ApiBaseClient(string apiUrl, Credentials credentials)
        {
            ApiUrl = apiUrl ?? "";
            _credentials = credentials;
        }

        public ApiBaseClient() { }

        /// <summary>
        /// Get authentication token based on data passed in constructor
        /// </summary>
        /// <returns>Token</returns>
        protected async Task<string> Authenticate(bool withRefresh = false)
        {
            if (_currentToken != null && !withRefresh)
                return _currentToken;

            _currentToken = await AuthenticateWithCustomCredentials(_credentials);
            return _currentToken;     
        }

        protected async Task<string> AuthenticateWithCustomCredentials(Credentials credentials)
        {

            if (string.IsNullOrEmpty(credentials?.ClientId) || string.IsNullOrEmpty(credentials?.ClientSecret))
                throw new BpsClientDataException("Client login data have not been provided");

            var response = await TryGetTokenAsync(credentials);

            if (response.status == HttpStatusCode.OK)
                return JsonSerializer.Deserialize<Token>(response.response).access_token;


            throw CreateBpsClientDataException(response.response, response.status);
        }

        private async Task<(string response, HttpStatusCode status)> TryGetTokenAsync(Credentials credentials)
        {
            var url = CreateRequestFullUrl("/api/oauth2/token");
            var parms = new Dictionary<string, string>
            {
                { "client_id", credentials.ClientId },
                { "client_secret", credentials.ClientSecret },
                { "grant_type", "client_credentials" }
            };
            var client = new HttpClient();
            var response = await client.PostAsync(url, new FormUrlEncodedContent(parms));
            return (await response.Content.ReadAsStringAsync(), response.StatusCode);
        }

        protected void OverrideCurrentToken(string token)
        {
            if(!string.IsNullOrEmpty(token))
                _currentToken = token;
        }
        
        protected string CreateRequestFullUrl(string requestUrl)
        {
            if (string.IsNullOrEmpty(ApiUrl) || string.IsNullOrEmpty(requestUrl))
                throw new BpsClientDataException("API url was not specified");
            if (ApiUrl.EndsWith("/")) 
                ApiUrl = ApiUrl.Substring(0, ApiUrl.Length - 1);

            ApiUrl = ApiUrl.Trim();
            if (ApiUrl.ToLower().Contains("/api") && requestUrl.ToLower().Contains("/api"))
               requestUrl = requestUrl.ToLower().Replace("/api", "");

            return ApiUrl + requestUrl;
        }

        /// <summary>
        /// Send request to specified url
        /// </summary>
        /// <param name="uri">Request url</param>
        /// <param name="data">Request body data</param>
        /// <param name="method">HTTP method</param>
        protected async Task<(string response, HttpStatusCode status)> SendRequestAsync(string uri, string data, string method = "POST")
        {
          if (string.IsNullOrEmpty(_currentToken))
              _currentToken = await Authenticate();
            
            var response = await GetResponseAsync(uri, data, _currentToken, method);
            if (response.status == HttpStatusCode.Unauthorized)
            {
                _currentToken = await Authenticate(true);
                response = await GetResponseAsync(uri, data, _currentToken, method);
            }

            return response;
        }

        protected async Task<(string response, HttpStatusCode status)> SendRequestWithCustomCredentialsAsync(string uri, string data, Credentials credentials, string method = "POST")
        {
            var token = await AuthenticateWithCustomCredentials(credentials);
            return await GetResponseAsync(uri, data, token, method);
        }

        /// <summary>
        /// Call async API method from url
        /// </summary>
        /// <param name="uri">API url</param>
        /// <param name="data">Request body</param>
        /// <param name="token">Authentication token</param>
        /// <param name="method">HTTP Method</param>
        /// <param name="contentType"></param>
        protected async Task<(string response, HttpStatusCode status)> GetResponseAsync(string uri, string data, string token, string method = "POST", string contentType = "application/json")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Proxy = null;
            request.ContentType = contentType;
            request.ContentLength = (!string.IsNullOrEmpty(data)) ? Encoding.UTF8.GetBytes(data).Length : 0;
            request.Method = method;
            if (!string.IsNullOrEmpty(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            if (data != null)
                using (StreamWriter requestBody = new StreamWriter(request.GetRequestStream()))
                    requestBody.Write(data);
            try
            {
                using HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);
                    return (reader.ReadToEnd(), response.StatusCode);
            }
            catch (WebException webex)
            {
                if (webex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (webex.Response is HttpWebResponse response)
                    {
                        using Stream respStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(respStream);
                        return (reader.ReadToEnd(), response.StatusCode);
                    }
                }
                WebResponse errResp = webex.Response;
                if (errResp != null)
                {
                    using Stream respStream = errResp.GetResponseStream();
                    StreamReader reader = new StreamReader(respStream);
                        throw new Exception(reader.ReadToEnd());
                }
                throw;
            }
        }
        protected BpsClientDataException CreateBpsClientDataException(string result, HttpStatusCode httpStatusCode)
        {
            try
            {
                if (string.IsNullOrEmpty(result)) return new BpsClientDataException($"HTTP Error - {(int)httpStatusCode} ({httpStatusCode})");
                ErrorResult errorResult = JsonSerializer.Deserialize<ErrorResult>(result);
                return new BpsClientDataException(errorResult.description);
            }
            catch
            {
                return new BpsClientDataException(result);
            }
        }
    }
    internal struct Token
    {
        public string access_token { get; set; }
    }
}
