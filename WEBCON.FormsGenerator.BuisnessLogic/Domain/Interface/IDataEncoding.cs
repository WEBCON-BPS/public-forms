namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Interface
{
    public interface IDataEncoding
    {
        string AppKey { get; set; }
        string Encode(string data);
        string Decode(string data);
    }
}
