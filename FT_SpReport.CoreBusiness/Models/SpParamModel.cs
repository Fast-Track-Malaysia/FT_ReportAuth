using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Models
{
    public class SpParamModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Seq { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string ParamName { get; set; }
        [Required]
        public SpParamTypeEnum ParamType { get; set; }
        public string ParamValue { get; set; }
        [JsonIgnore]
        [NotMapped]
        public decimal ParamDecimal { get; set; }
        [JsonIgnore]
        [NotMapped]
        public DateTime ParamDatetime { get; set; }
        [IgnoreDataMember]
        public virtual SpModel SpModel { get; set; }
        public string LookupName { get; set; }
    }

}
