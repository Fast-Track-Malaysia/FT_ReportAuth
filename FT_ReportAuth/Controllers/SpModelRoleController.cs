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
using Microsoft.AspNetCore.Identity;
using Dapper;

namespace FT_ReportAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpModelRoleController : ControllerBase
    {
        public SpModelRoleController(ApplicationDbContext myContext, IConfiguration configuration)
        {
            MyContext = myContext;
            Configuration = configuration;
        }

        private ApplicationDbContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpGet("{id:int}")]
        public IActionResult CustomGet(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    string sql = @"select * from SpModelRoles T0
                        inner join SpModels T1 on T0.SpModelId = T1.Id
                        where T0.Id = {0}";
                    sql = string.Format(sql, id.ToString());
                    var sp = conn.Query<SpModelRole>(sql).ToArray();
                    return Ok(sp);
                }
                //return Ok(JsonConvert.SerializeObject(sp));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }
    }
}
