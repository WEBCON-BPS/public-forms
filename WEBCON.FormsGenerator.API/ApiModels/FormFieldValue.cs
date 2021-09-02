using System.Data;

namespace WEBCON.FormsGenerator.API.ApiModels
{
    class FormFieldValue
    {
        public DataTable data { get; set; }
        public Column[] columns { get; set; }
    }

    class Column
    {
        public string sourceColumnName { get; set; }
        public string displayName { get; set; }
        public string type { get; set; }
    }

    class FormFieldValueData
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}