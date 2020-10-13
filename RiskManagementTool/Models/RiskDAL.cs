using RiskManagementTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RiskManagementTool.Models
{
    public class RiskDAL
    {
        string connectionString = "Data Source=188.121.44.214;Initial Catalog=Riskmanager;Persist Security Info=True;User ID=riskmanager;Password=P3nf01d99";

        //Get All Risks from DB
        public IEnumerable<RiskInfo> GetRisks()
        {
            List<RiskInfo> RiskList = new List<RiskInfo>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllRisks", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
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
                con.Close();
            }
            return RiskList;
        }

        // Insert Risk Into DB
        public void AddRisk(RiskInfo ris)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertRisk", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RiskSummary", ris.RiskSummary);
                cmd.Parameters.AddWithValue("@RiskDescription", ris.RiskDescription);
                cmd.Parameters.AddWithValue("@RiskRating", ris.RiskRating);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        // Update Risk Record

        public void UpdateRisk(RiskInfo ris)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateRisk", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RiskID", ris.ID);
                cmd.Parameters.AddWithValue("@RiskSummary", ris.RiskSummary);
                cmd.Parameters.AddWithValue("@RiskDescription", ris.RiskDescription);
                cmd.Parameters.AddWithValue("@RiskRating", ris.RiskRating);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        // Delete Risk Record

        public void DeleteRisk(int? risid)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_DeleteRisk", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RiskID", risid);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        // Get Risk Record

        public RiskInfo GetRiskById(int? risid)
        {
            RiskInfo ris = new RiskInfo();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetRisk", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RiskID", risid);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ris.ID = Convert.ToInt32(dr["ID"].ToString());
                    ris.RiskSummary = dr["RiskSummary"].ToString();
                    ris.RiskDescription = dr["RiskDescription"].ToString();
                    ris.RiskRating = Convert.ToDecimal(dr["RiskRating"].ToString());

                }
                con.Close();
            }
            return ris;
        }
    }
}
