namespace ServiceAggregator.Options
{
    public class MyOptions
    {
        public string ConnectionString { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
         public MyOptions() 
        {
        }
    }
}
