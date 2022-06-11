namespace GlobusTest.ViewModels
{
    public class ResponseMessage
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }

    public class ResponseMessage<T>
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public T Data { get; set; }
    }
}
