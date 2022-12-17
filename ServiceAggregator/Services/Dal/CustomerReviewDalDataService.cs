﻿using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class CustomerReviewDalDataService : DalDataServiceBase<CustomerReview, CustomerReviewDalDataService>, ICustomerReviewDalDataService
    {
        public CustomerReviewDalDataService(ICustomerReviewRepo mainRepo) : base(mainRepo)
        {
        }
    }
}