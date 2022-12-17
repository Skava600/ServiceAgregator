﻿namespace ServiceAggregator.Models
{
    public class DoerData
    {
        public Guid Id { get; set; }
        public string DoerName { get; set; }
        public string DoerDescription { get; set; }
        public int OrderCount { get; set; }
        public List<SectionData> Sections { get; set; }
    }
}