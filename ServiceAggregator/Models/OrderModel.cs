using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace ServiceAggregator.Models
{
    public class OrderModel
    {
        public string Header { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public DateTime ExpireDate { get; set; }
        public double? Price { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public Guid CustomerId { get; set; }
        public string Slug { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public Guid SectionId { get; set; }
    }
}
