﻿using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class CustomerData
    {
        public Guid Id { get; set; } 
        public double Rating
        {
            get
            {
                var rate = (double)Reviews.Sum(r => r.Grade) / Reviews.Count;
                return double.IsNaN(rate) ? 0: rate;
            }
        }
        public AccountData Account { get; set; }
        public List<ReviewData> Reviews { get; set; } = new List<ReviewData>();
    }
}
