namespace CwkSocial.Api.Contracts.Common
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string StatusPhrase { get; set; }
        public List<string> Erros { get; set; } = new List<string>();
        public DateTime TimeStamp { get; set; }
    }
}
