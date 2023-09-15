using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OpicxoInterviewTask.Models
{
    public class Sql
    {
        private static string constr = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
        public SqlConnection cn = new SqlConnection(constr);
        public DataTable Getdata(string qry)
        {
            DataTable dt = new DataTable();
            try
            {
                cn.Open();
                SqlDataAdapter ad = new SqlDataAdapter(qry, cn);
                ad.Fill(dt);
            }
            catch (Exception)
            {
            }
            finally
            {
                cn.Close();
            }
            return dt;
        }
        public int INSERT_UPDATE_DELETE(Person obj)
        {
            int i = 0;
            try
            {

                SqlCommand com = new SqlCommand("INSERT_UPDATE_DELETE", cn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", obj.Id);
                com.Parameters.AddWithValue("@PersonName", obj.PersonName);
                com.Parameters.AddWithValue("@PersonHeight", obj.PersonHeight);
                com.Parameters.AddWithValue("@PersonWeight", obj.PersonWeight);
                com.Parameters.AddWithValue("@Gender", obj.Gender);
                com.Parameters.AddWithValue("@ACTION", obj.ACTION);

                cn.Open();
                i = com.ExecuteNonQuery();
                cn.Close();                
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                cn.Close();
            }
            return i;
        }
        public int INSERT_UPDATE_DELETE_Activities(Activities obj)
        {
            int i = 0;
            try
            {

                SqlCommand com = new SqlCommand("INSERT_UPDATE_DELETE_Activities", cn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@PersonId", obj.PersonId);
                com.Parameters.AddWithValue("@Id", obj.Id);
                com.Parameters.AddWithValue("@ActivityDate", obj.ActivityDate);
                com.Parameters.AddWithValue("@WakeUpTime", obj.WakeUpTime);
                com.Parameters.AddWithValue("@IsGym", obj.IsGym);
                com.Parameters.AddWithValue("@IsMeditation", obj.IsMeditation);
                com.Parameters.AddWithValue("@MeditationMinutes", obj.MeditationMinutes);
                com.Parameters.AddWithValue("@IsRead", obj.IsRead);
                com.Parameters.AddWithValue("@ReadPages", obj.ReadPages);
                com.Parameters.AddWithValue("@ACTION", obj.ACTION);

                cn.Open();
                i = com.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception e)
            {

            }
            finally
            {
                cn.Close();
            }
            return i;
        }
        public DataTable GetListViewActivities(int PersonId, int? IsAll)
        {
            var sqlCommand = new SqlCommand();
            var ds = new DataTable();
            var sqlDataAdapter = new SqlDataAdapter();

            sqlCommand.CommandText = "[ListViewActivities]";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = cn;
            try
            {
                sqlCommand.Parameters.AddWithValue("@PersonId", PersonId);
                sqlCommand.Parameters.AddWithValue("@IsAll", IsAll);

                cn.Open();
                sqlDataAdapter.SelectCommand = sqlCommand;

                sqlDataAdapter.Fill(ds);                
            }
            catch (Exception ex)
            {
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }
    }
}