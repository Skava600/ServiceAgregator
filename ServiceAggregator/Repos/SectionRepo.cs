using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using System.Data;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class SectionRepo : BaseRepo<Section>, ISectionRepo
    {
        public SectionRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override async Task<IEnumerable<Section>> GetAll()
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.getallsections()";
            List<Section> workSections = new List<Section>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        workSections.Add(new Section
                        {
                           Id = reader.GetGuid(0),
                           Name = reader.GetString(1),
                           Slug = reader.GetString(2),
                           CategoryId = reader.GetGuid(3),
                        });
                    }
                }
            }

            CloseConnection();

            return workSections;
        }




        public override async Task<Section?> Find(Guid id)
        {
            OpenConnection();

            Section? workSection = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM public.get_section_by_id(" +
                $"'{id}');", _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        workSection = new Section
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Slug = reader.GetString(2),
                            CategoryId = reader.GetGuid(3),
                        };
                    }
                }
            }

            CloseConnection();

            return workSection;
        }

        public async Task<IEnumerable<Section>> GetSectionsByDoerIdAsync(Guid doerId)
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.get_sections_by_doer_id(" +
                $"'{doerId}');";
            List<Section> workSections = new List<Section>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        workSections.Add(new Section
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Slug = reader.GetString(2),
                            CategoryId = reader.GetGuid(3),
                        });
                    }
                }
            }

            CloseConnection();

            return workSections;
        }
    }
}
