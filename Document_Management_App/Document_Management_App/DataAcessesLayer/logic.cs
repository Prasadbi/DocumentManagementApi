using Document_Management_App.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using Document_Management_App.Models;
using System.Collections.Generic;

namespace Document_Management_App.DataAcessesLayer
{
  
    public class logic
    {
        string connectionstring = @"Data Source=VS_PRATIKN\Pratik,49172;Initial Catalog=DocumentApp;User ID=pratik;Password=pratikn";

        public void AddRequest(Request request)
        {
            int ID = Convert.ToInt32(request.Related_Document_Id);

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("SaveRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Request_Message",request.Request_Message);
                cmd.Parameters.AddWithValue("@Document_ID", ID);
                cmd.Parameters.AddWithValue("@status","1");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }
        //public List<Admin> getdata()
        //{
        //    List<Admin> admins = new List<Admin>();
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        SqlCommand cmd = new SqlCommand("GetDetails",con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        SqlDataReader rd = cmd.ExecuteReader();
        //       while(rd.Read())
        //        {
        //            Admin admin = new Admin();
        //            admin.Admin_Email = rd["Admin_Email"].ToString();
        //            admin.Admin_Password = rd["Admin_Password"].ToString();
        //            admins.Add(admin);
        //        }
        //    }
        //    return admins;
        //}

      public int CheckAdmin(Admin admin)
        {
            int check = 0;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
             
                SqlCommand cmd = new SqlCommand("CheckAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Admin_Email", admin.Admin_Email);
                cmd.Parameters.AddWithValue("@Admin_Password", admin.Admin_Password);
                con.Open();

                SqlDataReader rd = cmd.ExecuteReader();
               
                while (rd.Read())
                {
                    check = 1;
                   
                }
             
            }
            return check;
        }
       

    }
}
