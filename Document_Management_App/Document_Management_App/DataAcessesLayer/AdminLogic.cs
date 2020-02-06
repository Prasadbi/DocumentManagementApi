using Document_Management_App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Document_Management_App.DataAcessesLayer
{
    public class AdminLogic
    {
      
            SqlConnection con = new SqlConnection(@"Data Source=VS_PRATIKN\Pratik,49172;Initial Catalog=DocumentApp;User ID=pratik;Password=pratikn");

        public int Add_Document(string DocName, string DocUploadDate, string DocData, string Doctype, string DocPrivacy, string EmpId, string Docstatus)
        {

            int message=1;
            try
            {
                if (DocPrivacy == "common")
                {
                    EmpId = "DEMO";
                }

                string[] filename;

                filename = JsonConvert.DeserializeObject<string[]>(DocName);

                string[] fileData;

                fileData = JsonConvert.DeserializeObject<string[]>(DocData);


                for (int i = 0; i < fileData.Length; i++)
                {
                    string path= @"E:/Project/Document-Management-App/DocumentManagementApi/Document_Management_App/Document_Management_App/Documents/";

                    // string content = Convert.ToBase64String(File.ReadAllBytes(fileData[i]));

                    // File.WriteAllBytes(@"c:\yourfile", Convert.FromBase64String(yourBase64String));

                    var createfolder = Path.Combine(path);
                    //    System.IO.Directory.CreateDirectory(createfolder);

                    string[] file;

                    file = fileData[i].Split(',');
                   // var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(file[1]);

                    File.SetAttributes(createfolder, FileAttributes.Normal);

                    File.WriteAllBytes(createfolder+"/"+filename[i], Convert.FromBase64String(file[1]));

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
            catch (SqlException e)
            {
                message =0;
              
            }
            return message;
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

        public List<Employee> Get_All_Emplyee()
        {
            string status = "1";
            SqlCommand cmd = new SqlCommand("Select * from tbl_Employee with(nolock) where Emp_Status='" + status + "'", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Employee> emplist = new List<Employee>();
            while (reader.Read())
            {
                Employee employee = new Employee();

                employee.Emp_Id = reader[0].ToString();
                employee.Emp_Comp_Id = reader[1].ToString();
                employee.Emp_First_Name = reader[2].ToString();
                employee.Emp_Last_Name = reader[3].ToString();
                employee.Emp_Email = reader[4].ToString();
                employee.Emp_Password = reader[5].ToString();
                employee.Emp_Status = reader[6].ToString();


                emplist.Add(employee);
            }
            reader.Close();
            con.Close();
            return emplist;
        }
    }
}
