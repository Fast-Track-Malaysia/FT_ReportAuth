using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FT_SpReport.CoreBusiness.Models
{
    public class LookupModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LookUpName { get; set; }
        public string LookUpSQL { get; set; }
    }
}
