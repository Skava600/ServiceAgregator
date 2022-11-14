namespace ServiceAggregator.Entities.Base
{
    public class User : BaseEntity
    {
        public Account Account { get; set; }
        public Guid AccountId { get; set; }
    }
}
