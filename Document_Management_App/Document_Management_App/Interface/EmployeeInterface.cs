using Document_Management_App.Model;
using Document_Management_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.Interface
{
   
        public interface EmployeeInterface
        {
            void AddRequest(Request request);
            List<perticularDocument> perticularDocument(string document,string Emp_Comp_Id);
            string CheckAdmin(Admin admin);
            List<AllDocument> AllDocuments();
            List<AllDocument> GetALLDocumentForRequest(string empid);
            List<perticularDocument> GetPrivateDocument(string EmployeeId);
            void LogOutEmployee(string Emp_Comp_Id);
        }
    
}
