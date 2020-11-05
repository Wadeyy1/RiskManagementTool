using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RiskManagementTool.Models
{
    public class RiskDAL
    {
        //Creates Connection String from Data packet in AppSettings
        private SqlConnection connectionString;

        public RiskDAL()
        {
            var configuration = GetConfiguration();
            connectionString = new SqlConnection(configuration.GetSection("Data").GetSection("ConnectionString").Value);
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        //Checks if user if valid
        public int LoginCheck(UserLogin user)
        {
            SqlCommand cmd = new SqlCommand("SP_Login", connectionString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.Add("@IsValid", SqlDbType.Bit).Direction = ParameterDirection.Output;

            connectionString.Open();
            cmd.ExecuteNonQuery();

            int Result = Convert.ToInt32(cmd.Parameters["@IsValid"].Value);
            connectionString.Close();

            return Result;
        }

        public List<RiskSummary> RiskSummary()
        {
            connectionString.Open();

            List<RiskSummary> RiskSummary = new List<RiskSummary>();
            string strSelect = "Select RiskRating,Count(RiskRating) as RiskRatingCount From riskmanager.Risks Group By RiskRating";
            SqlCommand cmd = new SqlCommand(strSelect, connectionString);
            SqlDataReader myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                RiskSummary ris = new RiskSummary();
                ris.RiskRatingTotal = Convert.ToDecimal(myReader["RiskRating"].ToString());
                ris.RiskRatingTotalCount = Convert.ToInt32(myReader["RiskRatingCount"].ToString());
                RiskSummary.Add(ris);
            }

            myReader.Close();
            connectionString.Close();

            return RiskSummary;
        }

        //Get All Risks from DB
        public IEnumerable<RiskInfo> GetRisks()
        {
            List<RiskInfo> RiskList = new List<RiskInfo>();

            using (connectionString)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GetAllRisks", connectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connectionString.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        RiskInfo ris = new RiskInfo();
                        ris.ID = Convert.ToInt32(dr["ID"].ToString());
                        ris.RiskSummary = dr["RiskSummary"].ToString();
                        ris.RiskDescription = dr["RiskDescription"].ToString();
                        ris.RiskRating = Convert.ToDecimal(dr["RiskRating"].ToString());

                        RiskList.Add(ris);
                    }
                    connectionString.Close();
                }
                catch (Exception sqlCall)
                {
                    Console.WriteLine(sqlCall.Message);
                }
            }
            return RiskList;
        }

        // Insert Risk Into DB
        public void AddRisk(RiskInfo ris)
        {
            using (connectionString)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertRisk", connectionString);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RiskSummary", ris.RiskSummary);
                    cmd.Parameters.AddWithValue("@RiskDescription", ris.RiskDescription);
                    cmd.Parameters.AddWithValue("@RiskRating", ris.RiskRating);

                    connectionString.Open();
                    cmd.ExecuteNonQuery();
                    connectionString.Close();
                }
                catch (Exception sqlCall)
                {
                    Console.WriteLine(sqlCall.Message);
                }
            }
        }

        // Update Risk Record

        public void UpdateRisk(RiskInfo ris)
        {
            using (connectionString)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_UpdateRisk", connectionString);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RiskID", ris.ID);
                    cmd.Parameters.AddWithValue("@RiskSummary", ris.RiskSummary);
                    cmd.Parameters.AddWithValue("@RiskDescription", ris.RiskDescription);
                    cmd.Parameters.AddWithValue("@RiskRating", ris.RiskRating);

                    connectionString.Open();
                    cmd.ExecuteNonQuery();
                    connectionString.Close();
                }
                catch (Exception sqlCall)
                {
                    Console.WriteLine(sqlCall.Message);
                }
            }
        }

        // Delete Risk Record

        public void DeleteRisk(int? risid)
        {
            using (connectionString)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_DeleteRisk", connectionString);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RiskID", risid);

                    connectionString.Open();
                    cmd.ExecuteNonQuery();
                    connectionString.Close();
                }
                catch (Exception sqlCall)
                {
                    Console.WriteLine(sqlCall.Message);
                }
            }
        }

        // Get Risk Record

        public RiskInfo GetRiskById(int? risid)
        {
            RiskInfo ris = new RiskInfo();

            using (connectionString)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GetRisk", connectionString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RiskID", risid);
                    connectionString.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ris.ID = Convert.ToInt32(dr["ID"].ToString());
                        ris.RiskSummary = dr["RiskSummary"].ToString();
                        ris.RiskDescription = dr["RiskDescription"].ToString();
                        ris.RiskRating = Convert.ToDecimal(dr["RiskRating"].ToString());
                    }
                    connectionString.Close();
                }
                catch (Exception sqlCall)
                {
                    Console.WriteLine(sqlCall.Message);
                }
            }
            return ris;
        }
    }
}