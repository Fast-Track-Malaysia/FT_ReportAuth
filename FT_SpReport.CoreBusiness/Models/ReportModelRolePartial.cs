using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using FT_SpReport.CoreBusiness.Models;

namespace FT_SpReport.CoreBusiness.Models
{
    public partial class ReportModelRolePartial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [IgnoreDataMember]
        public virtual ReportModel SpModel { get; set; }

    }
}
