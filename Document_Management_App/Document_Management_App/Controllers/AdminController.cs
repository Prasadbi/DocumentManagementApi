using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Document_Management_App.DataAcessesLayer;
using Document_Management_App.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Document_Management_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class AdminController : ControllerBase
    {
        AdminLogic adminLogic = new AdminLogic();

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [HttpPost]
        [Route("adddocument")]
        public int Post1(Documents documentdata)
        {

            // Documents documents = new Documents();

            //documents = JsonConvert.DeserializeObject<Documents>(documentdta);

            int message;

            message= adminLogic.Add_Document(documentdata.Document_Name, documentdata.Document_Upload_Date, documentdata.Document_Data, documentdata.Document_Type, documentdata.Document_Privacy, documentdata.Emp_Comp_Id, documentdata.Document_Status);

            return message;
        }

        [HttpGet]
        [Route("GetPerticularDocument/{Doctype}")]
        public string getPerticularDocument(string Doctype)
        {


            List<Documents> perticular_documents = adminLogic.Get_Perticular_Doc(Doctype);
            return JsonConvert.SerializeObject(perticular_documents);

        }

       

        [HttpGet]
        [Route("GetAllEmplyee")]
        public string getAllEployee()
        {


            List<Employee> allemplyee = adminLogic.Get_All_Emplyee();
            return JsonConvert.SerializeObject(allemplyee);

        }

        [HttpGet]
        [Route("GetAllRequests")]
        public string getAllRequests()
        {


            List<RequestsByEmp> allRequests = adminLogic.Get_RequestsByEmp();
            return JsonConvert.SerializeObject(allRequests);

        }

        [HttpPut]
        [Route("DeleteDocument/{docname}/{docid}")]
        public void markMessegeRead(string docname,string docid)
        {
            adminLogic.DeleteDocument(docname,docid);
        }

        [HttpGet]
        [Route("GetAllSchedules")]
        public string getAllSchedules()
        {

            List<ScheduledMeeting> allSchedules = adminLogic.Get_getAllSchedules();
            return JsonConvert.SerializeObject(allSchedules);

        }

        [HttpGet]
        [Route("AllDocumentType")]
        public string getAllDocumentType()
        {


            List<DocumentTypes> perticular_documents = adminLogic.Get_AllDocumentType();
            return JsonConvert.SerializeObject(perticular_documents);

        }

        [HttpPost]
        [Route("addMeeting")]
        public int AddMeetingSchedule(ScheduledMeeting scheduledmeetings)
        {

            // Documents documents = new Documents();

            //documents = JsonConvert.DeserializeObject<Documents>(documentdta);

            int message;

            message = adminLogic.Add_MeetingShedule(scheduledmeetings);

            if (message == 1)
            {
                adminLogic.UpdateRequestTbl(scheduledmeetings.Request_Id);
            }

            return message;
        }

        

             [HttpPut]
        [Route("Update_Scheduled_Meeting_Status/{MeetingId}")]
        public void Update_Scheduled_Meeting_Status(string MeetingId)
        {
            adminLogic.update_Scheduled_Meeting_Status(MeetingId);
        }

    }
}