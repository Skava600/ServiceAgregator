using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceAggregator.Models
{
    public class ReviewModel
    {
        public Guid AuthorId { get; set; }
        public Guid OrderId { get; set; }
        public string Text { get; set; }
        public short Grade { get; set; }

        public Guid ReviewedId { get; set; }
    }
}
