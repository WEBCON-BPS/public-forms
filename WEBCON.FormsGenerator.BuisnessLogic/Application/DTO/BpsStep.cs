using System.Text.Json.Serialization;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.DTO
{
    public class BpsStep : BaseObject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
