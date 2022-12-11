using Npgsql;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Repos.Interfaces;
using System.Data;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class WorkSectionRepo : BaseRepo, IWorkSectionRepo
    {
        public WorkSectionRepo(string connectionString) : base(connectionString)
        {
        }

        public async Task<int> Add(WorkSectionModel workSection)
        {
            int id = -1;
            OpenConnection();
            string sql = "SELECT public.createworksection(" +
                $"'{workSection.Name}'," +
            $"'{workSection.CategoryName}');";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
            {
                id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            CloseConnection();

            return id;
        }

        public Task<int> Delete(WorkSection entity)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<WorkSection>> GetAll()
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.getallworksections()";
            List<WorkSection> workSections = new List<WorkSection>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        workSections.Add(new WorkSection
                        {
                           Id = reader.GetInt32(0),
                           Name = reader.GetString(1),
                           CategoryName = reader.GetString(2)
                        });
                    }
                }
            }

            CloseConnection();

            return workSections;
        }

        public Task<int> Update(WorkSection entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddRange(IEnumerable<WorkSectionModel> workSections)
        {
            int id = -1;
            OpenConnection();
            foreach (var workSection in workSections)
            {
                string sql = "SELECT public.createworksection(" +
                    $"'{workSection.Name}'," +
                $"'{workSection.CategoryName}');";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
                {
                    id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
            CloseConnection();

            return id;
        }

        public async Task<WorkSection?> Find(int? id)
        {

            if (id == null) return null;
            OpenConnection();

            WorkSection? workSection = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("public.getaccount", _sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        workSection = new WorkSection
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            CategoryName = reader.GetString(2)
                        };
                    }
                }
            }

            CloseConnection();

            return workSection;
        }
    }
}
