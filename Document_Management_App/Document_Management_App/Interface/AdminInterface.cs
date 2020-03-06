using Document_Management_App.Model;
using Document_Management_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Interface
{
        public interface AdminInterface
        {
            int Add_Document(Documents documents);
            List<Documents> Get_Perticular_Doc(string doctype);
            void DeleteDocument(string docid, string docname);
            List<Employee> Get_All_Emplyee();
            int getDocumentcount();
            List<RequestsByEmp> Get_RequestsByEmp();
            List<ScheduledMeeting> Get_getAllSchedules();
            List<DocumentTypes> Get_AllDocumentType();
            int Add_MeetingShedule(ScheduledMeeting scheduledmeeting);
            void UpdateRequestTbl(string requestId);
            void update_Scheduled_Meeting_Status(string meetinid);
            void SendMail(string requestId);
            int AddNewEmployee(Employee employee);
          void ShareDocument(ShareDocument sharedocument);
        List<ShareDocEmpList> getDocShareedListOfEmp(string DocId);

        }
}
