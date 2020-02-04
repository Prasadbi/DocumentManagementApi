using System;
using Document_Management_App.DataAcessesLayer;
using Document_Management_App.Model;
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

    }
}




