using System;
using System.Collections.Generic;
using System.Text;
using FT_SpReport.CoreBusiness.Helpers;

namespace FT_SpReport.CoreBusiness.Models
{
    public class SpRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Applied { get; set; }
        public bool EnableEdit{ get { return Name != StaticValues.AdministratorRole; } }
    }

}
