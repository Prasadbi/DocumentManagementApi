using System.Collections.Generic;
using System.Linq;
using Document_Management_App.DataAcessesLayer;
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
        logic Logic = new logic();
        [HttpPost]
        [Route("Postdata")]
        public void Create(Request request)//for query request
        {
        
                Logic.AddRequest(request);
                
        }
        //[HttpGet]
        //[Route("Postdata")]
        //public List<Admin> Get()
        //{
        //    List<Admin> admins = new List<Admin>();

        //    admins = Logic.getdata().ToList();
        //    return admins;
           
        //}


        [HttpPost]
        [Route("Postdata1")]
        public int Check(Admin admin)//for admin login
        {

            return Logic.CheckAdmin(admin);
          
        }

        [HttpPost]
        [Route("GetDocument")]
        public string Documents(Document document)//for  gettting all documents
        {
            List<Document> documents = new List<Document>();
            documents = Logic.GetData(document.Document_Type).ToList();
            string str = JsonConvert.SerializeObject(documents);
            return str;

        }

        //[HttpPost]
        //[Route("Postdata2")]
        //public int Check1(Employee employee)//for admin login
        //{

        //    return Logic.CheckEmployee(employee);

        //}

    }
}




