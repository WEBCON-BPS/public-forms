namespace WEBCON.FormsGenerator.API.ApiModels
{
    class NewElement
    {
        public Field[] formFields { get; set; }
    }
    class Field : IdentityModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public string svalue { get; set; }
        public object value { get; set; }
        public FormLayout formLayout { get; set; }
    }

    class FormLayout
    {
        public string editability { get; set; }
    }
}
