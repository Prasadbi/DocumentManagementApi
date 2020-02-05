using System.Collections.Generic;
using System.Linq;
using Document_Management_App.DataAcessesLayer;
using Document_Management_App.Model;
using Document_Management_App.Models;
using Microsoft.AspNetCore.Mvc;


namespace Document_Management_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        logic Logic = new logic();
        [HttpPost]
        [Route("Postdata")]
        public void Create(Request request)
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
        public int Check(Admin admin)
        {

            return Logic.CheckAdmin(admin);

        }
    }
}




