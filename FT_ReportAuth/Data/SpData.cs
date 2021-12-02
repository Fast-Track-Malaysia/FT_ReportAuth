using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using FT_SpReport.CoreBusiness.Models;

namespace FT_ReportAuth.Data
{
    public class SpData : SpDataPartial
    {
        [IgnoreDataMember]
        public virtual Microsoft.AspNetCore.Identity.IdentityUser User { get; set; }
    }
}
