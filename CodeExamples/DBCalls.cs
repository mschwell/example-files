using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruit.Models.DBCalls
{
    public class DBCalls
    {
        private RecruitEntities db = new RecruitEntities();

        public DBCalls()
        {

        }

        public DataTable GetMonthlyCandidateAdded()
        {

            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetCandidateCreatedPast12Months", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 5000;


                    sqlConnection.Open();
                    sqlAdapter.SelectCommand = sqlCommand;
                    sqlAdapter.Fill(dt);
                    sqlConnection.Close();
                }
            }

            return dt;
        }

        public DataTable GetMonthlyCandidateHired()
        {

            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetCandidateHiredPast12Months", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 5000;


                    sqlConnection.Open();
                    sqlAdapter.SelectCommand = sqlCommand;
                    sqlAdapter.Fill(dt);
                    sqlConnection.Close();
                }
            }

            return dt;
        }
        public DataTable GetAddedCandidatesChart(DateTime StartDate, DateTime EndDate)
        {
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetCandidateCreateCounts", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 5000;

                    sqlCommand.Parameters.Add("@start", SqlDbType.DateTime).Value = StartDate;
                    sqlCommand.Parameters.Add("@end", SqlDbType.DateTime).Value = EndDate;

                    sqlConnection.Open();
                    sqlAdapter.SelectCommand = sqlCommand;
                    sqlAdapter.Fill(dt);
                    sqlConnection.Close();
                }
            }

            return dt;
        }




        public DataTable GetCreatedJobOrderChart(DateTime StartDate, DateTime EndDate)
        {
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetJobOrderCreateCounts", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 5000;

                    sqlCommand.Parameters.Add("@start", SqlDbType.DateTime).Value = StartDate;
                    sqlCommand.Parameters.Add("@end", SqlDbType.DateTime).Value = EndDate;

                    sqlConnection.Open();
                    sqlAdapter.SelectCommand = sqlCommand;
                    sqlAdapter.Fill(dt);
                    sqlConnection.Close();
                }
            }

            return dt;
        }


        public DataTable GetMonthlyJobOrderAdded()
        {

            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetJobOrderCreatedPast12Months", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 5000;


                    sqlConnection.Open();
                    sqlAdapter.SelectCommand = sqlCommand;
                    sqlAdapter.Fill(dt);
                    sqlConnection.Close();
                }
            }

            return dt;
        }


        public  void SaveJobViewCount(int joborder_id, int site_id)
        {
            try
            {
                joborder_views jviews = new joborder_views();
                jviews.site_id = site_id;
                jviews.joborder_id = joborder_id;
                jviews.view_date = DateTime.Now;

                db.joborder_views.Add(jviews);
                db.SaveChanges();
            }
            catch (Exception) { }
        }
    }
}
