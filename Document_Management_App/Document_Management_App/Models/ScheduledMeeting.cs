using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Models
{
    public class ScheduledMeeting
    {
        public string Meeting_Id { get; set; }
        public string Emp_Comp_Id { get; set; }
        public string Meeting_Date { get; set; }
        public string Meeting_Time { get; set; }
        public string Request_Id { get; set; }
        public string Meeting_Room { get; set; }
        public string Emp_First_Name { get; set; }
        public string Emp_Last_Name { get; set; }
        public string Document_Name { get; set; }

    }
}
