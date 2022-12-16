﻿using Microsoft.Extensions.Options;
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
        public override Task<int> Delete(Section entity)
        {
            throw new NotImplementedException();
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

        public override Task<int> Update(Section entity)
        {
            throw new NotImplementedException();
        }




        public override async Task<Section?> Find(Guid id)
        {
            OpenConnection();

            Section? workSection = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("public.get_section_by_id", _sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("w_id", id);
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

    }
}