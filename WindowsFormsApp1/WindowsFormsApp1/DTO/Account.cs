using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DTO
{
    public class Account
    {
        public Account(int id, string displayName, string phoneNumber)
        {
            this.ID = id;         
            this.DisplayName = displayName;
            this.PhoneNumber = phoneNumber;
        }

        public Account(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DisplayName = row["DisplayName"].ToString();
            this.PhoneNumber = row["PhoneNumber"] != DBNull.Value ? row["PhoneNumber"].ToString() : "";
        }

        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }


        private string displayName;
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }


        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
    }
}
