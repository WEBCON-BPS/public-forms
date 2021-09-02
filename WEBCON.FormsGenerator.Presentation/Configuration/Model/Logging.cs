namespace WEBCON.FormsGenerator.Presentation.Configuration.Model
{
    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }
    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; } = "Warning";
    }
}
