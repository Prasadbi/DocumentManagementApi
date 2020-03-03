using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Models
{
    public class DBConnection
    {
        private string _dbconnection;
        public string dbconnection
        {
            get
            { return _dbconnection; }
            set
            { _dbconnection = value; }
        }
    }
}
