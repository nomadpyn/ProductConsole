using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace productconsole.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ContactPerson { get; set; }

        public override string ToString()
        {
            return $"{this.Id} - {this.Name} - {this.ContactPerson}";
        }
    }
}
