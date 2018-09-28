using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruit.Models.Ranking
{
    public static class CandidateRanking
    {
        public static async Task RunCandidateRanking()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var asyncConnectionString = new SqlConnectionStringBuilder(connectionString)
            {
                AsynchronousProcessing = true
            }.ToString();

            using (var conn = new SqlConnection(asyncConnectionString))
            {
                using (var cmd = new SqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "RunRanking";
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
