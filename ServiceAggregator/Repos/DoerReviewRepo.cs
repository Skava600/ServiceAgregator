using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class DoerReviewRepo : BaseRepo<DoerReview>, IDoerReviewRepo
    {
        public DoerReviewRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override Task Delete(DoerReview entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DoerReview>> GetDoersReviews(Guid doerId)
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.get_doers_reviews(" +
                $"'{doerId}')";
            List<DoerReview> reviews = new List<DoerReview>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        reviews.Add(new DoerReview
                        {
                            Id = reader.GetGuid(0),
                            Grade = reader.GetInt16(1),
                            Text = reader.GetString(2),
                            OrderId = reader.GetGuid(3),
                            CustomerAuthorId = reader.GetGuid(4),
                            DoerId = reader.GetGuid(5),
                        });
                    }
                }
            }

            CloseConnection();

            return reviews;
        }
    }
}
