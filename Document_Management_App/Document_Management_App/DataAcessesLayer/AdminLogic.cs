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
                    EmpId = "common";
                }

                string[] filename;

                filename = JsonConvert.DeserializeObject<string[]>(DocName);

                string[] fileData;

                fileData = JsonConvert.DeserializeObject<string[]>(DocData);


                for (int i = 0; i < fileData.Length; i++)
                {
                    // string content = Convert.ToBase64String(File.ReadAllBytes(fileData[i]));
                    //    System.IO.Directory.CreateDirectory(createfolder);
                    // File.WriteAllBytes(@"c:\yourfile", Convert.FromBase64String(yourBase64String));
                    // var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(file[1]);

                    string path = @"E:/Project/Document-Management-App/DocumentManagementApi/Document_Management_App/Document_Management_App/Documents/";

                    var createfolder = Path.Combine(path);

                    string[] file;

                    file = fileData[i].Split(',');

                    string FILEDATA = file[1];

                    int documentcnt = getDocumentcount();

                    string documentcntString = documentcnt.ToString();



                    string[] filenamesplit = filename[i].Split('.');

                    string filename_Without_Extention = filenamesplit[0];
                    string file_Extention = filenamesplit[1];

                    string NewFileName= filename_Without_Extention+"_"+ documentcntString+ "."+file_Extention;

                    File.SetAttributes(createfolder, FileAttributes.Normal);

                    File.WriteAllBytes(createfolder+"/"+NewFileName, Convert.FromBase64String(FILEDATA));

                    SqlCommand cmd = new SqlCommand("Add_Document", con);

                    cmd.Parameters.AddWithValue("@Document_Name", NewFileName);
                    // cmd.Parameters.AddWithValue("@Document_Upload_Date", DocUploadDate);
                    //cmd.Parameters.AddWithValue("@Document_Data", fileData[i]);
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

                string path = @"E:/Project/Document-Management-App/DocumentManagementApi/Document_Management_App/Document_Management_App/Documents/";

                string filename = reader[1].ToString();
                string contents;

                foreach (string file in Directory.EnumerateFiles(path, filename))
                {
                    Byte[] bytes = File.ReadAllBytes(path+filename);
                    contents = "data: text / plain; base64,"+Convert.ToBase64String(bytes);

                    Documents documents = new Documents();

                    documents.Document_Id = reader[0].ToString();
                    documents.Document_Name = reader[1].ToString();
                    documents.Document_Upload_Date = reader[2].ToString();
                    documents.Document_Data = contents;
                    documents.Document_Type = reader[3].ToString();
                    documents.Document_Privacy = reader[4].ToString();
                    documents.Emp_Comp_Id = reader[5].ToString();
                    documents.Document_Status = reader[6].ToString();


                    perticular_doc_data.Add(documents);

                }
          
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

        public int getDocumentcount()
        {
            int documentcount=0;
            SqlCommand cmd = new SqlCommand("Select max(Document_Id) from tbl_Documents with(nolock)", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string d = reader[0].ToString();
                if (d.Equals(""))
                {
                    documentcount = 1;
                }
                else
                {
                    documentcount = Convert.ToInt32(reader[0]);
                    documentcount = documentcount + 1;
                }
                
            }
            con.Close();
            return documentcount;
        }

        public List<RequestsByEmp> Get_RequestsByEmp()
        {
          
            SqlCommand cmd = new SqlCommand("GetAllRequests", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<RequestsByEmp> request_list = new List<RequestsByEmp>();
            while (reader.Read())
            {
                RequestsByEmp requestsByEmp = new RequestsByEmp();

                requestsByEmp.Request_Id = reader[0].ToString();
                requestsByEmp.Emp_Comp_Id = reader[1].ToString();
                requestsByEmp.Emp_First_Name = reader[2].ToString();
                requestsByEmp.Emp_Last_Name = reader[3].ToString();
                requestsByEmp.Request_Message = reader[4].ToString();
                requestsByEmp.Related_Document_Id = reader[5].ToString();
                requestsByEmp.Document_Name = reader[6].ToString();
                requestsByEmp.Request_Date = reader[7].ToString();
                requestsByEmp.Requst_status = reader[8].ToString();

                request_list.Add(requestsByEmp);
            }
            reader.Close();
            con.Close();
            return request_list;
        }

        public List<ScheduledMeeting> Get_getAllSchedules()
        {
            SqlCommand cmd = new SqlCommand("GetAllScheduledMeetings", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<ScheduledMeeting> scheduledMeetings_list = new List<ScheduledMeeting>();
            while (reader.Read())
            {
                ScheduledMeeting scheduledMeeting = new ScheduledMeeting();

                scheduledMeeting.Meeting_Id = reader[0].ToString();
                scheduledMeeting.Emp_Comp_Id = reader[1].ToString();
                scheduledMeeting.Meeting_Date = reader[2].ToString();
                scheduledMeeting.Meeting_Time = reader[3].ToString();
                scheduledMeeting.Request_Id = reader[4].ToString();
                scheduledMeeting.Meeting_Room = reader[5].ToString();
                scheduledMeeting.Emp_First_Name = reader[6].ToString();
                scheduledMeeting.Emp_Last_Name = reader[7].ToString();
                scheduledMeeting.Document_Name = reader[8].ToString();




                scheduledMeetings_list.Add(scheduledMeeting);
            }
            reader.Close();
            con.Close();
            return scheduledMeetings_list;
        }

    }
}
