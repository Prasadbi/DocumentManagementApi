using System.Collections.Generic;
using System.Linq;
using Document_Management_App.DataAcessesLayer;
using Document_Management_App.Interface;
using Document_Management_App.Model;
using Document_Management_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Document_Management_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController(EmployeeInterface dbOps)
        {
            DBRecords = dbOps;
        }
        public EmployeeInterface DBRecords { get; set; }


       
        [HttpPost]
        [Route("PostRequest")]
        public void Create(Request request)//for query request
        {

            DBRecords.AddRequest(request);

        }


        [HttpPost]
        [Route("PostdataForAdmin")]
        public string Check(Admin admin)//for admin login
        {

            return DBRecords.CheckAdmin(admin);

        }

        [HttpPost]
        [Route("GetPerticularDocument")]
        public string Documents(perticularDocument document)//for gettting all perticular documents documents
        {
            List<perticularDocument> documents = new List<perticularDocument>();
            documents = DBRecords.perticularDocument(document.Document_Type).ToList();
            string str = JsonConvert.SerializeObject(documents);
            return str;

        }

        [HttpGet]
        [Route("GetAllDocument")]
        public List<AllDocument> Documents1()//for gettting all documents
        {
            List<AllDocument> documents = new List<AllDocument>();
            documents = DBRecords.AllDocuments().ToList();
            //string str = JsonConvert.SerializeObject(documents);
            return documents;

        }

       
        [HttpGet]
        [Route("GetAllDocumentsForRequest/{empid}")]
        public List<AllDocument> getdocumentsForDropDown(string empid)
        {
            List<AllDocument> documents = new List<AllDocument>();
            documents = DBRecords.GetALLDocumentForRequest(empid).ToList();
            return documents;
        }

        [HttpPost("{Employee_Id}")]
        public List<perticularDocument> getdocuments(string Employee_Id)
        {
            List<perticularDocument> documents = new List<perticularDocument>();
            documents = DBRecords.GetPrivateDocument(Employee_Id);
            return documents;
        }


        [HttpPut]
        [Route("LogOutEmp/{Employeedata}")]
        public void LogOutEmp(string Employeedata)
        {

            DBRecords.LogOutEmployee(Employeedata);
        }

        
    }
}



