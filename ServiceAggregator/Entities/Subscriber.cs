namespace ServiceAggregator.Entities
{
    public class Subscriber : ORM.DbInstance
    {
        public DateTime SubscribeExpireDate { get; set; }
    }
}
