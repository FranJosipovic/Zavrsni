namespace Zavrsni.TeamOps
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public object? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
