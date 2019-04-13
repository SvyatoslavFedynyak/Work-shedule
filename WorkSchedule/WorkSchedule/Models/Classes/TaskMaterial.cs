using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public enum MaterialType
    {LINK, FILE}

    public class TaskMaterial
    {
        MaterialType type;
        string uri;

        public TaskMaterial() { }
        public TaskMaterial(MaterialType type, string uri)
        {
            this.type = type;
            this.uri = uri;
        }

        public MaterialType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public string Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
            }
        }
    }
}
