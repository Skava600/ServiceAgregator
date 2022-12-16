namespace ServiceAggregator.Entities.Serializable
{
    public class CategorySerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> sections { get; set; }
        public CategorySerializable() { 
                
        }
    }
}
