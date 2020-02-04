﻿using Document_Management_App.Model;
using System;
using System.Data;
using System.Data.SqlClient;


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
        
    }
}
