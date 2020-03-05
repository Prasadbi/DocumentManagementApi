using Document_Management_App.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using Document_Management_App.Models;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Net.Mail;
using Document_Management_App.Interface;
using Document_Management_App.DBContext;
using Microsoft.Extensions.Options;

namespace Document_Management_App.DataAcessesLayer
{
    public class logic : EmployeeInterface
    {
        // string connectionstring = @"Data Source=VS_PRATIKN\Pratik,49172;Initial Catalog=DocumentApp;User ID=pratik;Password=pratikn";

        private readonly ConnectionStrings connectionStrings;
        private readonly ConnectionStrings connectionStrings1;


        public logic(IOptions<ConnectionStrings> options)
        {
            connectionStrings = options.Value;
        }

        public void AddRequest(Request request)
        {
            string ConnectionPath = connectionStrings.connectionstrings;
            int i = 1;
            int ID = Convert.ToInt32(request.Related_Document_Id);
            try
            {

                using (SqlConnection con = new SqlConnection(ConnectionPath))
                {
                    SqlCommand cmd = new SqlCommand("SaveRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Employee_Comp_ID", request.Emp_Comp_Id);
                    cmd.Parameters.AddWithValue("@Request_Message", request.Request_Message);
                    cmd.Parameters.AddWithValue("@Document_ID", ID);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                Console.Write("request is not valid:" + ex);
                i = 0;
            }
            if (i == 1)
            {
                using (SqlConnection con = new SqlConnection(ConnectionPath))
                {
                    string Fname = "", Lname = "", email = "", DocumentName = "";
                    SqlCommand cmd = new SqlCommand("GetEmployeeById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Employee_Comp_ID", request.Emp_Comp_Id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Fname = Convert.ToString(reader[2]);
                        Lname = Convert.ToString(reader[3]);
                        email = Convert.ToString(reader[4]);
                        // password = Convert.ToString(rd[5]);
                    }
                    reader.Close();

                    SqlCommand command1 = new SqlCommand("GetDocumentByDocumentId", con);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@Document_ID", Convert.ToInt32(ID));

                    //con.Open();
                    command1.ExecuteNonQuery();
                    SqlDataReader reader1 = command1.ExecuteReader();
                    while (reader1.Read())
                    {

                        DocumentName = Convert.ToString(reader1[1]);
                    }
                    reader1.Close();

                    string subject = "Disscution About Document Query";

                    string body = "Hi madam/sir, my name is" + Fname + ",i have query in " + DocumentName + " " + " Thank You";


                    string FromMail = "amarjadhav738779@gmail.com";
                    // string emailTo = "reciever@reckonbits.com.pk";
                    MailMessage mail = new MailMessage();

                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    mail.From = new MailAddress(FromMail);

                    mail.To.Add("pbidwai@versionsolutions.com");
                    mail.Subject = subject;
                    mail.Body = body;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("amarjadhav738779@gmail.com", "Amar@9552065205");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Port = 587;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    // SmtpServer.UseDefaultCredentials = true;
                    SmtpServer.Send(mail);
                }
            }


        }
        public List<perticularDocument> perticularDocument(string document,string empid)
        {

            string ConnectionPath = connectionStrings.connectionstrings;
            using (SqlConnection con = new SqlConnection(ConnectionPath))
            {

                // string status = "1";
                // string privcy = "common";
                SqlCommand cmd = new SqlCommand("GetPerticularDocumentNew", con);
                // SqlCommand cmd = new SqlCommand("Select * from tbl_Documents with(nolock) where Document_Type='" + document + "' and Document_Status='" + status + "' and Document_Privacy='"+privcy+"'", con);
                cmd.Parameters.AddWithValue("@Document_Type", document);
                cmd.Parameters.AddWithValue("@Emp_Comp_Id", empid);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                List<perticularDocument> perticular_doc_data = new List<perticularDocument>();
                string path = @"E:/Project/Document-Management-App/DocumentManagementApi/Document_Management_App/Document_Management_App/Documents/";

                while (rd.Read())
                {

                    string filename = Convert.ToString(rd[1]);
                    string contents;

                    foreach (string file in Directory.EnumerateFiles(path, filename))
                    {
                        Byte[] bytes = File.ReadAllBytes(path + filename);
                        contents = "data: text / plain; base64," + Convert.ToBase64String(bytes);

                        perticularDocument doc = new perticularDocument();
                        {

                            doc.Document_Id = Convert.ToString(rd[0]);
                            doc.Document_Name = Convert.ToString(rd[1]);
                            doc.Document_Upload_Date = Convert.ToDateTime(rd["Document_Upload_Date"]);
                            doc.Document_Data = contents;
                            doc.Document_Type = Convert.ToString(rd[3]);
                            doc.Document_Privacy = Convert.ToString(rd[4]);
                            doc.Emp_Comp_Id = Convert.ToString(rd[5]);
                            // doc.Document_Status = rd[6].ToString();

                        }
                        perticular_doc_data.Add(doc);

                    }
                }
                rd.Close();
                con.Close();
                return perticular_doc_data;
            }
        }

        public string CheckAdmin(Admin admin)
        {
            string ConnectionPath = connectionStrings.connectionstrings;
            int check = 0;
            string Verification_Data = "";
            using (SqlConnection con = new SqlConnection(ConnectionPath))
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
                    LoginVerification verification = new LoginVerification();
                    verification.verfication = Convert.ToString(check);
                    verification.Employee_Comp_Id = Convert.ToString(rd[0]);
                    Verification_Data = JsonConvert.SerializeObject(verification);
                    AddLoginInfo(Convert.ToString(rd[0]));


                }
                con.Close();
            }
            if (check == 0)
            {
                //string ConnectionPath = connectionStrings.DbConnection;
                using (SqlConnection con = new SqlConnection(ConnectionPath))
                {

                    SqlCommand command = new SqlCommand("CheckEmployee", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Emp_Email", admin.Admin_Email);
                    command.Parameters.AddWithValue("@Emp_Password", admin.Admin_Password);
                    con.Open();

                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {

                        check = 2;
                        LoginVerification verification = new LoginVerification();
                        verification.verfication = Convert.ToString(check);
                        verification.Employee_Comp_Id = Convert.ToString(reader[1]);
                        Verification_Data = JsonConvert.SerializeObject(verification);
                        AddLoginInfo(Convert.ToString(reader[1]));

                    }
                    con.Close();
                }

            }
            return Verification_Data;
        }

        public List<AllDocument> AllDocuments()
        {
            string ConnectionPath = connectionStrings.connectionstrings;
            List<AllDocument> l = new List<AllDocument>();
            using (SqlConnection con = new SqlConnection(ConnectionPath))
            {
                SqlCommand cmd = new SqlCommand("GetAllDocument", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    AllDocument document = new AllDocument();
                    document.Document_Type_Id = Convert.ToString(rd[0]);
                    document.Document_Type_Name = Convert.ToString(rd[1]);

                    l.Add(document);
                };
                con.Close();
            }
            return l;
        }

        public List<AllDocument> GetALLDocumentForRequest(string empid)
        {
            string ConnectionPath = connectionStrings.connectionstrings;
            // string status = "1";
            List<AllDocument> l = new List<AllDocument>();
            using (SqlConnection con = new SqlConnection(ConnectionPath))
            {
                SqlCommand cmd = new SqlCommand("GetPerticDocumentForReq", con);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@Emp_Comp_Id", empid);
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    AllDocument document = new AllDocument();
                    {
                        document.Document_Type_Id = Convert.ToString(rd[0]);
                        document.Document_Type_Name = Convert.ToString(rd[1]);
                        //document.status = Convert.ToString(rd[6]);
                    }
                    l.Add(document);
                };
                con.Close();
            }
            return l;

        }
        public List<perticularDocument> GetPrivateDocument(string EmployeeId)
        {
            string ConnectionPath = connectionStrings.connectionstrings;
            using (SqlConnection con = new SqlConnection(ConnectionPath))
            {

                SqlCommand cmd = new SqlCommand("GetPrivateDocument", con);
                cmd.Parameters.AddWithValue("@Employee_Comp_Id", EmployeeId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                List<perticularDocument> perticular_doc_data = new List<perticularDocument>();
                string path = @"E:/Project/Document-Management-App/DocumentManagementApi/Document_Management_App/Document_Management_App/Documents/";
                while (rd.Read())
                {

                    string filename = Convert.ToString(rd[1]);
                    string contents;

                    foreach (string file in Directory.EnumerateFiles(path, filename))
                    {
                        Byte[] bytes = File.ReadAllBytes(path + filename);
                        contents = "data: text / plain; base64," + Convert.ToBase64String(bytes);

                        perticularDocument doc = new perticularDocument();
                        {

                            doc.Document_Id = Convert.ToString(rd[0]);
                            doc.Document_Name = Convert.ToString(rd[1]);
                            doc.Document_Upload_Date = Convert.ToDateTime(rd["Document_Upload_Date"]);
                            doc.Document_Data = contents;
                            doc.Document_Type = Convert.ToString(rd[3]);
                            doc.Document_Privacy = Convert.ToString(rd[4]);
                            doc.Emp_Comp_Id = Convert.ToString(rd[5]);
                            // doc.Document_Status = rd[6].ToString();

                        }
                        perticular_doc_data.Add(doc);

                    }
                }
                rd.Close();
                con.Close();
                return perticular_doc_data;
            }
        }

      public void  AddLoginInfo(string Emp_Comp_Id)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings1);
            try
            {
                SqlCommand cmd = new SqlCommand("insert into tbl_DocumentApp_LoginInfo(Emp_Comp_Id,Login_Date_Time) values('"+Emp_Comp_Id+"',getDate())", con);
                con.Open();  
                cmd.ExecuteNonQuery();
                con.Close();
               
            }
            catch (Exception e)
            {
              
            }
        }


        public void LogOutEmployee(string Emp_Comp_Id)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings1);
            try
            {
                SqlCommand cmd = new SqlCommand("Update tbl_DocumentApp_LoginInfo set Login_Status=0 where Emp_Comp_Id='"+Emp_Comp_Id+"'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {

            }
        }
    }
}