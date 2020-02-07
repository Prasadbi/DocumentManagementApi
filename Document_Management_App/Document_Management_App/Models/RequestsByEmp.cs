﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Models
{
    public class RequestsByEmp
    {
        public string Request_Id { get; set; }
        public string Emp_Comp_Id { get; set; }
        public string Emp_First_Name { get; set; }
        public string Emp_Last_Name { get; set; }
        public string Request_Message { get; set; }
        public string Related_Document_Id { get; set; }
        public string Document_Name { get; set; }
        public string Request_Date { get; set; }
        public string Requst_status { get; set; }
    }
}