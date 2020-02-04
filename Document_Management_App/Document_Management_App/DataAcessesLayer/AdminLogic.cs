using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Document_Management_App.DataAcessesLayer
{
    public class AdminLogic
    {
        SqlConnection con = new SqlConnection(@"Data Source=VS_PRATIKN\Pratik,49172;Initial Catalog=DocumentApp;User ID=pratik;Password=pratikn");
        public void Add_Document(string DocName,string DocUploadDate,string DocData,string Doctype,string DocPrivacy,string EmpId,string Docstatus)
        {
            if (DocPrivacy == "common")
            {
                EmpId = "EMPOOOO";
            }

           string[] filename;

            filename = JsonConvert.DeserializeObject<string[]>(DocName);

            string[] fileData;

            fileData = JsonConvert.DeserializeObject<string[]>(DocData);


            for (int i = 0; i < fileData.Length; i++)
            {
                SqlCommand cmd = new SqlCommand("Add_Document", con);

                cmd.Parameters.AddWithValue("@Document_Name", filename[i]);
                // cmd.Parameters.AddWithValue("@Document_Upload_Date", DocUploadDate);
                cmd.Parameters.AddWithValue("@Document_Data", fileData[i]);
                cmd.Parameters.AddWithValue("@Document_Type", Doctype);
                cmd.Parameters.AddWithValue("@Document_Privacy", DocPrivacy);
                cmd.Parameters.AddWithValue("@Emp_Comp_Id", EmpId);
                cmd.Parameters.AddWithValue("@Document_Status", "1");

                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                con.Close();
            }

           

        }
    }
}
