using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class DoerWorkSection
    {
        public int DoerId { get; set; }
        public Doer Doer { get; set; }

        public int WorkSectionId { get; set; }
        public WorkSection WorkSection { get; set; }
    }
}
