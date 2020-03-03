using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Model
{
    public class Request
    {
        public string Emp_Comp_Id { get; set; }
        public string Request_Message { get; set; }     
        public string Requst_status { get; set; }
        public string Related_Document_Id { get; set; }
        public DateTime Request_Date { get; set; }

    }
}
