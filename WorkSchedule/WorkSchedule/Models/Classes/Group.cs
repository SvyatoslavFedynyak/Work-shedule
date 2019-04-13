using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class Group
    {
        string name;
        List<User> members;

        public Group() { }
        public Group(string name, List<User> members)
        {
            this.name = name;
            this.members = members;
        }

        public List<User> Members
        {
            get
            {
                return members;
            }
            set
            {
                members = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}
