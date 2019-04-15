using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class User
    {
        int id;
        protected string login;
        protected string password;
        protected int groupId;

        public User() { }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        public int GroupID
        {
            get { return groupId; }
            set { groupId = value; }
        }

    }
}
