namespace ServiceAggregator.Entities.Base
{
    public class User : BaseEntity
    {
        public Account Account { get; set; }
        public int AccountId { get; set; }
    }
}
