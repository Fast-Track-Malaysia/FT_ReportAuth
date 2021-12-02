using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FT_SpReport.CoreBusiness.Models
{
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public string AuthData { get; set; }
    }
}
