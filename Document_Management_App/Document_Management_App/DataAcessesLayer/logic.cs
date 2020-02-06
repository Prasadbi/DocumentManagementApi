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
                cmd.Parameters.AddWithValue("@Request_Message", request.Request_Message);
                cmd.Parameters.AddWithValue("@Document_ID", ID);
                cmd.Parameters.AddWithValue("@status", "1");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }
        public List<Document> GetData(string document)
        {
            List<Document> documents = new List<Document>();
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("GetDocument", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Document_Type", document);
                con.Open();

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Document doc = new Document();
                    {
                        doc.Document_Name = rd["Document_Name"].ToString();
                        doc.Document_Upload_Date = Convert.ToDateTime(rd["Document_Upload_Date"]);
                        doc.Document_Data = rd["Document_Data"].ToString();
                        doc.Document_Type = rd["Document_Type"].ToString();
                        doc.Document_Privacy = rd["Document_Privacy"].ToString();
                        doc.Document_Status = rd["Document_Status"].ToString();
                        doc.Emp_Comp_Id = rd["Emp_Comp_Id"].ToString();
                    };
                    documents.Add(doc);
                }
                con.Close();
            }
            return documents;
        }

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
                con.Close();
            }
            if (check == 0)
            {
                using (SqlConnection con1 = new SqlConnection(connectionstring))
                {

                    SqlCommand cmd1 = new SqlCommand("CheckEmployee", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@Emp_Email", admin.Admin_Email);
                    cmd1.Parameters.AddWithValue("@Emp_Password", admin.Admin_Password);
                    con1.Open();

                    SqlDataReader rd1 = cmd1.ExecuteReader();

                    while (rd1.Read())
                    {
                        check = 2;

                    }
                    con1.Close();
                }

            }
            return check;
        }

    }
}
