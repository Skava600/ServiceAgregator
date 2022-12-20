﻿using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IDoerSectionRepo : IBaseRepo<DoerSection>
    {
        public Task DeleteDoerSectionsByDoerId(Guid doerId );
    }
}
