using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Models
{
    public class SpModel
    {
        public SpModel()
        {
            this.Details = new HashSet<SpParamModel>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string SpName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string SpDesciption { get; set; }
        [Required]
        public string SpSql { get; set; }

        public ICollection<SpParamModel> Details { get; set; }

    }

}
