using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Models
{
    public class RequestsByEmp
    {
        public int Request_Id { get; set; }
        public int Emp_Comp_Id { get; set; }
        public int Emp_First_Name { get; set; }
        public int Emp_Last_Name { get; set; }
        public int Request_Message { get; set; }
        public int Related_Document_Id { get; set; }
        public int Document_Name { get; set; }
        public int Request_Date { get; set; }
        public int Requst_status { get; set; }
    }
}