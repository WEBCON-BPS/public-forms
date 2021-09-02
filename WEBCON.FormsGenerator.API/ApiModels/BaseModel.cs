namespace WEBCON.FormsGenerator.API.ApiModels
{
    class BaseModel : IdentityModel
    {
        public string name { get; set; }
        public Links[] links { get; set; }
    }
}
