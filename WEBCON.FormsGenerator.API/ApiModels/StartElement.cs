namespace WEBCON.FormsGenerator.API.ApiModels
{
    internal class StartElement
    {
        public IdentityModel workflow { get; set; }
        public IdentityModel formType { get; set; }
        public IdentityModel businessEntity { get; set; }
        public int? parentInstanceId { get; set; }
        public StartElementFormField[] formFields { get; set; }
    }
    internal class StartElementResult
    {
        public int id { get; set; }
        public string instanceNumber { get; set; }
        public string status { get; set; }
    }
    internal class StartElementFormField : IdentityModel
    {
        public object value { get; set; }
        public string type { get; set; }
        public string svalue { get; set; }
        public string name { get; set; }
    }
    internal class StartElementValueChoices
    {
        public StartElementValueChoice[] choices { get; set; }
    }
    internal class StartElementValueChoice
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
