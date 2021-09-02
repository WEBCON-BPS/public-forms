namespace WEBCON.FormsGenerator.API.ApiModels
{
    class FormLayouts
    {
        public FormLayoutField[] fields { get; set; }
    }
    class FormLayoutField
    {
        public FormLayoutFieldConfiguration configuration { get; set; }
        public int id { get; set; }
        public string guid { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string requiredness { get; set; }
        public string visibility { get; set; }
    }
    class FormLayoutFieldConfiguration
    {
        public bool? allowMultipleValues { get; set; }
        public bool? allowTypedInValue { get; set; }
        public FormLayoutScaleValues scaleValues { get; set; }
        public FormFieldValueData[] answers { get; set; }
    }
    class FormLayoutScaleValues
    {
        public short min { get; set; }
        public short max { get; set; }
    }
}
