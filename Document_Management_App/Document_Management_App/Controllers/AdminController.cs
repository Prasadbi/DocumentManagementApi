using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Document_Management_App.DataAcessesLayer;
using Document_Management_App.Interface;
using Document_Management_App.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Document_Management_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  

    public class AdminController : ControllerBase
    {

        public AdminController(AdminInterface dbOps)
        {
            DBRecords = dbOps;
        }
        public AdminInterface DBRecords { get; set; }


        [HttpPost]
        [Route("adddocument")]
        public int AddNewDocument(Documents documentdata)
        {
            int message;

            message= DBRecords.Add_Document(documentdata);

            return message;
        }

        [HttpGet]
        [Route("GetPerticularDocument/{Doctype}")]
        public string getPerticularDocument(string Doctype)
        {
          

            List<Documents> perticular_documents = DBRecords.Get_Perticular_Doc(Doctype);
            return JsonConvert.SerializeObject(perticular_documents);

        }

       

        [HttpGet]
        [Route("GetAllEmplyee")]
        public string getAllEployee()
        {
           
            List<Employee> allemplyee = DBRecords.Get_All_Emplyee();
            return JsonConvert.SerializeObject(allemplyee);

        }

        [HttpGet]
        [Route("GetAllRequests")]
        public string getAllRequests()
        {
           
            List<RequestsByEmp> allRequests = DBRecords.Get_RequestsByEmp();
            return JsonConvert.SerializeObject(allRequests);

        }

        [HttpPut]
        [Route("DeleteDocument/{docname}/{docid}")]
        public void markMessegeRead(string docname,string docid)
        {

            DBRecords.DeleteDocument(docname,docid);
        }

        [HttpGet]
        [Route("GetAllSchedules")]
        public string getAllSchedules()
        {
           
            List<ScheduledMeeting> allSchedules = DBRecords.Get_getAllSchedules();
            return JsonConvert.SerializeObject(allSchedules);

        }

        [HttpGet]
        [Route("AllDocumentType")]
        public string getAllDocumentType()
        {
           
            List<DocumentTypes> perticular_documents = DBRecords.Get_AllDocumentType();
            return JsonConvert.SerializeObject(perticular_documents);

        }

        [HttpPost]
        [Route("addMeeting")]
        public int AddMeetingSchedule(ScheduledMeeting scheduledmeetings)
        {
           
            // Documents documents = new Documents();

            //documents = JsonConvert.DeserializeObject<Documents>(documentdta);

            int message;

            message = DBRecords.Add_MeetingShedule(scheduledmeetings);

            if (message == 1)
            {
                DBRecords.UpdateRequestTbl(scheduledmeetings.Request_Id);

                DBRecords.SendMail(scheduledmeetings.Request_Id);


            }

            return message;
        }

        [HttpPut]
        [Route("Update_Scheduled_Meeting_Status/{MeetingId}")]
        public void Update_Scheduled_Meeting_Status(string MeetingId)
        {

            DBRecords.update_Scheduled_Meeting_Status(MeetingId);
        }

        [HttpPost]
        [Route("addNewEmployee")]
        public int addNewEmployee(Employee employee)
        {
            return DBRecords.AddNewEmployee(employee);
        }

    }
}