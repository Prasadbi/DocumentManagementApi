using Document_Management_App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Document_Management_App;
using Document_Management_App.Controllers;
using Document_Management_App.DBContext;
using Microsoft.Extensions.Options;
using Document_Management_App.Interface;
using System.Reflection.PortableExecutable;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System.Net;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;

namespace Document_Management_App.DataAcessesLayer
{
    public class AdminLogic:AdminInterface
    {
        private readonly ConnectionStrings connectionStrings;


        public AdminLogic(IOptions<ConnectionStrings> options)
        {
            connectionStrings = options.Value;
        }


        public int Add_Document(Documents documents)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings) ;
          
         

            int message =1;
            try
            {
                if (documents.Document_Privacy == "common")
                {
                    documents.Emp_Comp_Id = "common";
                }

                string[] filename;

                filename = JsonConvert.DeserializeObject<string[]>(documents.Document_Name);

                string[] fileData;

                fileData = JsonConvert.DeserializeObject<string[]>(documents.Document_Data);

  string path = @"E:/Project/Document-Management-App/DocumentManagementApi/Document_Management_App/Document_Management_App/Documents/";

                for (int i = 0; i < fileData.Length; i++)
                {
                    try
                    {
                        var createfolder = Path.Combine(path);

                        string[] file;

                        file = fileData[i].Split(',');

                        string FILEDATA = file[1];

                        int documentcnt = getDocumentcount();

                        string documentcntString = Convert.ToString(documentcnt);



                        string[] filenamesplit = filename[i].Split('.');

                        string filename_Without_Extention = filenamesplit[0];
                        string file_Extention = filenamesplit[1];

                        string NewFileName = filename_Without_Extention + "_" + documentcntString + "." + file_Extention;

                       // File.SetAttributes(createfolder, FileAttributes.Normal);

                        File.WriteAllBytes(createfolder + "/" + NewFileName, Convert.FromBase64String(FILEDATA));

                       

                        SqlCommand cmd = new SqlCommand("Add_Document", con);


                        cmd.Parameters.AddWithValue("@Document_Name", NewFileName);
                        // cmd.Parameters.AddWithValue("@Document_Upload_Date", DocUploadDate);
                        //cmd.Parameters.AddWithValue("@Document_Data", fileData[i]);
                        cmd.Parameters.AddWithValue("@Document_Type", documents.Document_Type);
                        cmd.Parameters.AddWithValue("@Document_Privacy", documents.Document_Privacy);
                        cmd.Parameters.AddWithValue("@Emp_Comp_Id", documents.Emp_Comp_Id);
                        //cmd.Parameters.AddWithValue("@Document_Status", "1");

                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        message = 0;
                    }
                }
            }
            catch (SqlException e)
            {

                message = 0;

            }
            return message;
       }

       public List<Documents> Get_Perticular_Doc(string doctype)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            //string status = "1";
            //string privacy = "common";
            SqlCommand cmd = new SqlCommand("GetPerticularDocument", con);
            cmd.Parameters.AddWithValue("@Document_Type",doctype);

            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Documents> perticular_doc_data = new List<Documents>();
            while (reader.Read())
            {
                List<ShareDocEmpList> list = getDocShareedListOfEmp(Convert.ToString(reader[0]));

                string path = @"E:/Project/Document-Management-App/DocumentManagementApi/Document_Management_App/Document_Management_App/Documents/";

                string filename = Convert.ToString(reader[1]);
                string contents;

                foreach (string file in Directory.EnumerateFiles(path, filename))
                {
                    Byte[] bytes = File.ReadAllBytes(path+filename);
                    contents = "data:text/plain;base64,"+Convert.ToBase64String(bytes);

                    Documents documents = new Documents();

                    documents.Document_Id = Convert.ToString(reader[0]);
                    documents.Document_Name = Convert.ToString(reader[1]);
                    documents.Document_Upload_Date = Convert.ToString(reader[2]);
                    documents.Document_Data = contents;
                    documents.Document_Type = Convert.ToString(reader[3]);
                    documents.Document_Privacy = Convert.ToString(reader[4]);
                    documents.Emp_Comp_Id = Convert.ToString(reader[5]);
                    documents.Document_Status = Convert.ToString(reader[6]);
                    documents.Document_ShareCount = Convert.ToString(list.Count);


                    perticular_doc_data.Add(documents);

                }

              


            }
            reader.Close();
            con.Close();
            return perticular_doc_data;

        }

        public void DeleteDocument(string docid , string docname)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
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

          

            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            //string status = "1";
            SqlCommand cmd = new SqlCommand("GetALLEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Employee> emplist = new List<Employee>();
            while (reader.Read())
            {
                Employee employee = new Employee();

                //  employee.Emp_Id =  Convert.ToString(reader[0]);
                employee.Emp_Comp_Id = Convert.ToString(reader[0]);
                employee.Emp_First_Name = Convert.ToString(reader[1]);
                employee.Emp_Last_Name = Convert.ToString(reader[2]);
                employee.Emp_Email = Convert.ToString(reader[3]);
                employee.Emp_Password = Convert.ToString(reader[4]);
                // employee.Emp_Status =  Convert.ToString(reader[5]);


                emplist.Add(employee);
            }
            reader.Close();
            con.Close();
            return emplist;
        }

        public int getDocumentcount()
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            int documentcount=0;
            SqlCommand cmd = new SqlCommand("GetDocumentMaxCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string d =Convert.ToString(reader[0]);

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
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            SqlCommand cmd = new SqlCommand("GetAllRequests", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<RequestsByEmp> request_list = new List<RequestsByEmp>();
            while (reader.Read())
            {
                RequestsByEmp requestsByEmp = new RequestsByEmp();

                requestsByEmp.Request_Id = Convert.ToString(reader[0]);
                requestsByEmp.Emp_Comp_Id = Convert.ToString(reader[1]);
                requestsByEmp.Emp_First_Name = Convert.ToString(reader[2]);
                requestsByEmp.Emp_Last_Name = Convert.ToString(reader[3]);
                requestsByEmp.Request_Message = Convert.ToString(reader[4]);
                requestsByEmp.Related_Document_Id = Convert.ToString(reader[5]);
                requestsByEmp.Document_Name = Convert.ToString(reader[6]);
                requestsByEmp.Request_Date = Convert.ToString(reader[7]);
                requestsByEmp.Requst_status =  Convert.ToString(reader[8]);
                request_list.Add(requestsByEmp);

            }
            reader.Close();
            con.Close();
            return request_list;
        }

        public List<ScheduledMeeting> Get_getAllSchedules()
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            SqlCommand cmd = new SqlCommand("GetAllScheduledMeetings", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<ScheduledMeeting> scheduledMeetings_list = new List<ScheduledMeeting>();
            while (reader.Read())
            {
                ScheduledMeeting scheduledMeeting = new ScheduledMeeting();

                DateTime Time = DateTime.ParseExact(Convert.ToString(reader[3]), "HH:mm",CultureInfo.InvariantCulture);

                string FormatedMeetingTime = Time.ToString("hh:mm tt");

                scheduledMeeting.Meeting_Id = Convert.ToString(reader[0]);
                scheduledMeeting.Emp_Comp_Id = Convert.ToString(reader[1]);
                scheduledMeeting.Meeting_Date = Convert.ToString(reader[2]);
                scheduledMeeting.Meeting_Time = FormatedMeetingTime;
                scheduledMeeting.Request_Id = Convert.ToString(reader[4]);
                scheduledMeeting.Meeting_Room = Convert.ToString(reader[5]);
                scheduledMeeting.Emp_First_Name = Convert.ToString(reader[6]);
                scheduledMeeting.Emp_Last_Name = Convert.ToString(reader[7]);
                scheduledMeeting.Document_Name = Convert.ToString(reader[8]);

                scheduledMeetings_list.Add(scheduledMeeting);
            }
            reader.Close();
            con.Close();
            return scheduledMeetings_list;
        }

        public List<DocumentTypes> Get_AllDocumentType()
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            SqlCommand cmd = new SqlCommand("GetAllDocument", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<DocumentTypes> DocumentTypes_list = new List<DocumentTypes>();
            while (reader.Read())
            {
                DocumentTypes documentTypes = new DocumentTypes();

                documentTypes.Document_Id = Convert.ToString(reader[0]);
                documentTypes.Document_Type_Name = Convert.ToString(reader[1]);


                DocumentTypes_list.Add(documentTypes);
            }
            reader.Close();
            con.Close();
            return DocumentTypes_list;
        }

        public int Add_MeetingShedule(ScheduledMeeting scheduledmeeting)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            int message = 1;

            try
            {
                SqlCommand cmd = new SqlCommand("Add_MeetingSchedule", con);

                cmd.Parameters.AddWithValue("@Emp_Comp_Id", scheduledmeeting.Emp_Comp_Id);

                cmd.Parameters.AddWithValue("@Meeting_Date", scheduledmeeting.Meeting_Date);
                cmd.Parameters.AddWithValue("@Meeting_Time", scheduledmeeting.Meeting_Time);
                cmd.Parameters.AddWithValue("@Request_Id", Convert.ToInt32(scheduledmeeting.Request_Id));
                cmd.Parameters.AddWithValue("@Meeting_Room", scheduledmeeting.Meeting_Room);
               // cmd.Parameters.AddWithValue("@Meeting_Status", "1");

                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                message = 0;
            }
           

            return message;
            }

        public void UpdateRequestTbl(string requestId)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            int RequestId = Convert.ToInt32(requestId);
            SqlCommand cmd = new SqlCommand("UpdateRequestStatus", con);
            cmd.Parameters.AddWithValue("@Request_Id", RequestId);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            con.Close();

        }


        public void update_Scheduled_Meeting_Status(string meetinid)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            int MeetinId = Convert.ToInt32(meetinid);
            SqlCommand cmd = new SqlCommand("Update_Scheduled_MeetingStatus", con);
            cmd.Parameters.AddWithValue("@Meeting_Id", MeetinId);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public async void SendMailAsync(string requestId)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            string name ="";
            string Employee_email = "";
            string MeetingDate = "";
            string MeetingTime = "";
            string MeetingRoom = "";
            string DocumentName = "";

            int RequestId = Convert.ToInt32(requestId);
            SqlCommand cmd = new SqlCommand("SendMailFromAdmin", con);
            cmd.Parameters.AddWithValue("@Request_Id", RequestId);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = Convert.ToString(reader[0]) + " " + Convert.ToString(reader[1]);
                    Employee_email = Convert.ToString(reader[2]);
                    MeetingDate = Convert.ToString(reader[3]);
                    MeetingTime = Convert.ToString(reader[4]);
                    MeetingRoom = Convert.ToString(reader[5]);
                    DocumentName = Convert.ToString(reader[6]);

                }

            con.Close();

            if (Employee_email != "" || MeetingDate != "" || MeetingTime != "" || DocumentName != "")
            {

                // Refer to the .NET quickstart on how to setup the environment:
                // https://developers.google.com/calendar/quickstart/dotnet
                // Change the scope to CalendarService.Scope.Calendar and delete any stored
                // credentials.

                UserCredential credential;

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                           new ClientSecrets
                           {
                               ClientId = "83891333767-9m2mt2r3jmcmlav4ged98t6ldehmot8d.apps.googleusercontent.com",
                               ClientSecret = "7WuJMRt3fIF0Z67q_pM67cn-"

                           },
                           new[] { CalendarService.Scope.CalendarEventsReadonly },
                           "amarjadhav738779@gmail.com",
                           CancellationToken.None,
                           new FileDataStore("Books.ListMyLibrary"));

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Quickstart",

                });


                Event newEvent = new Event()
                {
                    Summary = "Discussion About Document Query.",
                    Location = "Baner Biz Bay-1," + MeetingRoom,
                    Description = "Discussion About Document Query.",

                    Start = new EventDateTime()
                    {
                        DateTime = DateTime.Parse(MeetingDate + "T" + MeetingTime + "-07:00"),
                        TimeZone = "Asia/Kolkata",
                    },
                    End = new EventDateTime()
                    {
                        DateTime = DateTime.Parse(MeetingDate + "T" + MeetingTime+ "-07:00"),
                        TimeZone = "Asia/Kolkata",
                    },
                    Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=1" },
                    Attendees = new EventAttendee[] {
                         //new EventAttendee() { Email = Employee_email },
                         new EventAttendee() { Email = Employee_email },
       
                    },
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = true,
                   //     Overrides = new EventReminder[] {
                   //new EventReminder() { Method = "email", Minutes = 24 * 60 },
                   //new EventReminder() { Method = "sms", Minutes = 10 },
                   // }
                    }
                };

              //  newEvent.Visibility = "true";
             
                String calendarId = "primary";
              
                Google.Apis.Calendar.v3.EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
                Event createdEvent = request.Execute();
             



           


                DateTime Time = DateTime.ParseExact(MeetingTime, "HH:mm",
                                      CultureInfo.InvariantCulture);




                string FormatedMeetingTime = Time.ToString("hh:mm tt");

                string subject = "Discussion About Document Query";

                string body = "Hi, " + name + ",<br>We arrenge the meeting with you for disscuss about " + DocumentName + " " +
                              " document.<br> Meeting date is: " + MeetingDate + ", <br> Time is :" + FormatedMeetingTime + " and Meeting Room Is: " + MeetingRoom + "<br><i>Thank You</i>";


                string FromMail = "amarjadhav738779@gmail.com";
                // string emailTo = "reciever@reckonbits.com.pk";
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                mail.From = new MailAddress(FromMail);

                System.Net.NetworkCredential Credentials = new System.Net.NetworkCredential("amarjadhav738779@gmail.com", "Amar@9552065205");

                mail.To.Add(Employee_email);
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

        public int AddNewEmployee(Employee employee)
        {
            int flag = 0;

            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            try
            {
                SqlCommand cmd = new SqlCommand("Add_New_Employee", con);

                cmd.Parameters.AddWithValue("@Emp_Comp_Id", employee.Emp_Comp_Id);
                cmd.Parameters.AddWithValue("@Emp_First_Name", employee.Emp_First_Name);
                cmd.Parameters.AddWithValue("@Emp_Last_Name",employee.Emp_Last_Name);
                cmd.Parameters.AddWithValue("@Emp_Email", employee.Emp_Email);
                cmd.Parameters.AddWithValue("@Emp_Password",employee.Emp_Password );
              
                // cmd.Parameters.AddWithValue("@Meeting_Status", "1");

                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                con.Close();
                flag = 1;
            }
            catch (Exception e)
            {
                flag = 0;
            }
            return flag;
        }

       public void ShareDocument(ShareDocument sharedocument)
        {
            string[] Emp_Comp_Id_Share;

            Emp_Comp_Id_Share = JsonConvert.DeserializeObject<string[]>(sharedocument.Emp_Comp_Id_Share);


            for (int i = 0; i < Emp_Comp_Id_Share.Length; i++)
            {
                int docid = Convert.ToInt32(sharedocument.Document_Id);

                SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
              
                    SqlCommand cmd = new SqlCommand("ShareDocument", con);
                
                    cmd.Parameters.AddWithValue("@Document_Id", docid);
                    cmd.Parameters.AddWithValue("@Emp_Comp_Id", Emp_Comp_Id_Share[i]);

                    // cmd.Parameters.AddWithValue("@Meeting_Status", "1");

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    con.Close();
              
            }


            string[] Emp_Comp_Id_UnShare;

            Emp_Comp_Id_UnShare = JsonConvert.DeserializeObject<string[]>(sharedocument.Emp_Comp_Id_UnShare);


            for (int i = 0; i < Emp_Comp_Id_UnShare.Length; i++)
            {
                int docid = Convert.ToInt32(sharedocument.Document_Id);

                SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);

                SqlCommand cmd = new SqlCommand("UnShareDocument", con);

                cmd.Parameters.AddWithValue("@Document_Id", docid);
                cmd.Parameters.AddWithValue("@Emp_Comp_Id", Emp_Comp_Id_UnShare[i]);

                // cmd.Parameters.AddWithValue("@Meeting_Status", "1");

                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                con.Close();

            }


        }

        public List<ShareDocEmpList> getDocShareedListOfEmp(string DocId)
        {
            SqlConnection con = new SqlConnection(connectionStrings.connectionstrings);
            SqlCommand cmd = new SqlCommand("GetDocSharedEmpList", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Document_Id", DocId);
            SqlDataReader reader = cmd.ExecuteReader();

            List<ShareDocEmpList> docEmpListData = new List<ShareDocEmpList>();

            while (reader.Read())
            {
                ShareDocEmpList docEmpList = new ShareDocEmpList();
                docEmpList.Emp_First_Name = Convert.ToString(reader[0]);
                docEmpList.Emp_Last_Name = Convert.ToString(reader[1]);
                docEmpList.Document_ID = Convert.ToString(reader[2]);
                docEmpList.Document_Name = Convert.ToString(reader[3]);
                docEmpList.Document_Upload_Date = Convert.ToString(reader[4]);
                docEmpList.Document_Type = Convert.ToString(reader[5]);
                docEmpList.Document_Privacy = Convert.ToString(reader[6]);
                docEmpList.Emp_Comp_Id = Convert.ToString(reader[7]);
                docEmpList.Document_Status = Convert.ToString(reader[8]);

                docEmpListData.Add(docEmpList);

            }
            reader.Close();
            con.Close();
            return docEmpListData;
        }

    }
}
