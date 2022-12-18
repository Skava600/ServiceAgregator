using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class CustomerReviewRepo : BaseRepo<CustomerReview>, ICustomerReviewRepo
    {
        public CustomerReviewRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override Task Delete(CustomerReview entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerReview>> GetCustomersReviews(Guid customerId)
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.get_customers_reviews(" +
                $"'{customerId}')";
            List<CustomerReview> reviews = new List<CustomerReview>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        reviews.Add(new CustomerReview
                        {
                            Id = reader.GetGuid(0),
                            Grade = reader.GetInt16(1),
                            Text = reader.GetString(2),
                            OrderId = reader.GetGuid(3),
                            DoerAuthorId = reader.GetGuid(4),
                            CustomerId = reader.GetGuid(5),
                        });
                    }
                }
            }

            CloseConnection();

            return reviews;
        }
    }
}
