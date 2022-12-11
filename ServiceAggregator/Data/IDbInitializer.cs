namespace ServiceAggregator.Data
{
    public interface IDbInitializer
    {
        public Task Seed();
    }
}
