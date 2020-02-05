using Document_Management_App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.DataAcessesLayer
{
    public class AdminLogic
    {
        SqlConnection con = new SqlConnection(@"Data Source=VS_PRATIKN\Pratik,49172;Initial Catalog=DocumentApp;User ID=pratik;Password=pratikn");
        public void Add_Document(string DocName,string DocUploadDate,string DocData,string Doctype,string DocPrivacy,string EmpId,string Docstatus)
        {
            if (DocPrivacy == "common")
            {
                EmpId = "EMPOOOO";
            }

           string[] filename;

            filename = JsonConvert.DeserializeObject<string[]>(DocName);

            string[] fileData;

            fileData = JsonConvert.DeserializeObject<string[]>(DocData);


            for (int i = 0; i < fileData.Length; i++)
            {
                SqlCommand cmd = new SqlCommand("Add_Document", con);

                cmd.Parameters.AddWithValue("@Document_Name", filename[i]);
                // cmd.Parameters.AddWithValue("@Document_Upload_Date", DocUploadDate);
                cmd.Parameters.AddWithValue("@Document_Data", fileData[i]);
                cmd.Parameters.AddWithValue("@Document_Type", Doctype);
                cmd.Parameters.AddWithValue("@Document_Privacy", DocPrivacy);
                cmd.Parameters.AddWithValue("@Emp_Comp_Id", EmpId);
                cmd.Parameters.AddWithValue("@Document_Status", "1");

                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                con.Close();
            }

           

        }

       public List<Documents> Get_Perticular_Doc(string doctype)
        {
            string status = "1";
            SqlCommand cmd = new SqlCommand("Select * from tbl_Documents with(nolock) where Document_Type='" +doctype+ "' and Document_Status='"+status+"'", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Documents> perticular_doc_data = new List<Documents>();
            while (reader.Read())
            {
                Documents documents = new Documents();

                documents.Document_Id = reader[0].ToString();
                documents.Document_Name = reader[1].ToString();
                documents.Document_Upload_Date = reader[2].ToString();
                documents.Document_Data = reader[3].ToString();
                documents.Document_Type = reader[4].ToString();
                documents.Document_Privacy = reader[5].ToString();
                documents.Emp_Comp_Id = reader[6].ToString();
                documents.Document_Status = reader[7].ToString();


                perticular_doc_data.Add(documents);
            }
            reader.Close();
            con.Close();
            return perticular_doc_data;

        }

        public void DeleteDocument(string docid , string docname)
        {
            int newdocid = Convert.ToInt32(docid);
            SqlCommand cmd = new SqlCommand("DeleteDocument", con);
            cmd.Parameters.AddWithValue("@Document_Id", newdocid);
            cmd.Parameters.AddWithValue("@Document_Name", docname);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
