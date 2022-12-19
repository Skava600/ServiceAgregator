using System.Security.Policy;

namespace ServiceAggregator.Models
{
    public class DoerModel
    {
        public string DoerName { get; set; } = "";
        public string DoerDescription { get; set; } = "";
        public List<string> Filters { get; set; } = new List<string>();
    }
}
