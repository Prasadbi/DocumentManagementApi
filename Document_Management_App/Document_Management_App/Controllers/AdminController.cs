﻿using System;
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



        
        [Route("adddocument")]

        public void Post1(Documents documentdata)
        {
           // Documents documents = new Documents();

            //documents = JsonConvert.DeserializeObject<Documents>(documentdta);

            adminLogic.Add_Document(documentdata.Document_Name, documentdata.Document_Upload_Date, documentdata.Document_Data, documentdata.Document_Type, documentdata.Document_Privacy, documentdata.Emp_Comp_Id, documentdata.Document_Status);

           // return new string("Added Successfuly");
        }

        


    }
}