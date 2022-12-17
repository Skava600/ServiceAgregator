using ORM;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class DoerWorkSection : DbInstance
    {
        public Guid DoerId { get; set; }
        //public Doer Doer { get; set; }

        public Guid WorkSectionId { get; set; }
       // public WorkSection WorkSection { get; set; }
    }
}
