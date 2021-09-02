namespace WEBCON.FormsGenerator.BusinessLogic.Application.DTO
{
    public class Credentials
    {
        public Credentials(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
    }
}
