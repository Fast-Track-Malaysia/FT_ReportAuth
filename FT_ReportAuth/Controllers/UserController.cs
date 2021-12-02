using Dapper;
using FT_SpReport.CoreBusiness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FT_ReportAuth.Services;
using FT_ReportAuth.Data;
using FT_SpReport.CoreBusiness.Helpers;
using Microsoft.AspNetCore.Identity;

namespace FT_ReportAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(ApplicationDbContext myContext, IConfiguration configuration)
        {
            MyContext = myContext;
            Configuration = configuration;
        }
        private ApplicationDbContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpGet()]
        public IActionResult CustomGet()
        {
            try
            {
                string rtn = "No record found.";
                var sp = MyContext.Users;
                if (sp == null)
                {
                    return Problem(rtn);
                }

                return Ok(JsonConvert.SerializeObject(sp));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }

    }
}
