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

namespace FT_ReportAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportModelFilterController : ControllerBase
    {
        public ReportModelFilterController(ApplicationDbContext myContext, IConfiguration configuration)
        {
            MyContext = myContext;
            Configuration = configuration;
        }
        private ApplicationDbContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpGet("{username}")]
        public IActionResult CustomGet(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    string sql = @"select distinct T1.* from ReportModels T1 
                        inner join ReportModelRoles T2 on T1.Id = T2.SpModelId
                        inner join AspNetRoles T3 on T2.UserRoleId = T3.Id
                        inner join AspNetUserRoles T4 on T3.Id = T4.RoleId
                        inner join AspNetUsers T5 on T4.UserId = T5.Id
                        where T5.UserName = '{0}'";
                    sql = string.Format(sql, username);
                    var sp = conn.Query<ReportModel>(sql).ToArray();
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
