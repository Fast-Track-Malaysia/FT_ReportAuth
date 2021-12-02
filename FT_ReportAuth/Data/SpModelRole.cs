using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using FT_SpReport.CoreBusiness.Models;

namespace FT_ReportAuth.Data
{
    public class SpModelRole : SpModelRolePartial
    {
        [IgnoreDataMember]
        public virtual Microsoft.AspNetCore.Identity.IdentityRole UserRole { get; set; }

    }
}
