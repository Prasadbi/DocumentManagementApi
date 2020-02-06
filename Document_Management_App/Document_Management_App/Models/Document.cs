﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Models
{
    public class Document
    {
        public string Document_Name { get; set; }
        public DateTime Document_Upload_Date { get; set; }
        public string Document_Data { get; set; }
        public string Document_Type { get; set; }
        public string Document_Privacy { get; set; }
        public string Document_Status { get; set; }
        public string Emp_Comp_Id { get; set; }

    }
}
