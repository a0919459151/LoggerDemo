namespace LoggerDemo.DbEntities
{
    public class HttpClientLog
    {
        public int Id { get; set; }
        public string Timestamp { get; set; }
        public string Method { get; set; }
        public string Endpoint { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public string StatusCode { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
        public double ExecutionTime { get; set; }
    }
}
