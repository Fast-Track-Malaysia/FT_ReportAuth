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
    public class UserRoleController : ControllerBase
    {
        public UserRoleController(ApplicationDbContext myContext, IConfiguration configuration)
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
                var sp = MyContext.Roles;
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
        [HttpGet("{type}/{name}")]
        public IActionResult CustomGet(string type, string name)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    string sql = "";
                    if (type == SpRoleypeEnum.user.ToString())
                    {
                        sql = @"select convert(nvarchar, T0.Id) as Id, T0.Name, case when T2.RoleId is null then 0 else 1 end as Applied 
                            from AspNetRoles T0
                            left join 
                            (
                            select T1.RoleId
                            from AspNetUserRoles T1
                            inner join AspNetUsers T2 on T1.UserId = T2.Id and T2.UserName = '{0}'
                            ) T2 on T0.Id = T2.RoleId";
                        sql = string.Format(sql, name);
                    }
                    else if (type == SpRoleypeEnum.spmodel.ToString())
                    {
                        sql = @"select convert(nvarchar, T0.Id) as Id, T0.Name, case when T2.UserRoleId is null then 0 else 1 end as Applied 
                            from AspNetRoles T0
                            left join 
                            (
                            select T1.UserRoleId
                            from SpModelRoles T1
                            inner join SpModels T2 on T1.SpModelId = T2.Id and T2.SpName = '{0}'
                            ) T2 on T0.Id = T2.UserRoleId";
                        sql = string.Format(sql, name);
                    }
                    else if (type == SpRoleypeEnum.reportmodel.ToString())
                    {
                        sql = @"select convert(nvarchar, T0.Id) as Id, T0.Name, case when T2.UserRoleId is null then 0 else 1 end as Applied 
                            from AspNetRoles T0
                            left join 
                            (
                            select T1.UserRoleId
                            from ReportModelRoles T1
                            inner join ReportModels T2 on T1.SpModelId = T2.Id and T2.ReportName = '{0}'
                            ) T2 on T0.Id = T2.UserRoleId";
                        sql = string.Format(sql, name);
                    }
                    IEnumerable<SpRole> sp = conn.Query<SpRole>(sql);
                    return Ok(sp);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }
        [HttpPost()]
        public IActionResult CustomPut([FromBody] string sprole)
        {
            try
            {
                string rtn = "No record added.";
                var sp = MyContext.Roles.Where<IdentityRole>(pp => pp.Name == sprole).FirstOrDefault();
                if (sp != null)
                {
                    return Problem("Record Found");
                }

                IdentityRole role = new IdentityRole(sprole) { NormalizedName = sprole.ToUpper() };

                MyContext.Roles.Add(role);
                MyContext.SaveChanges();

                sp = MyContext.Roles.Where<IdentityRole>(pp => pp.Name == sprole).FirstOrDefault();
                if (sp == null)
                {
                    return Problem(rtn);
                }
                //SpRole spRole = new SpRole() { Id = sp.Id, Name = sp.Name, Applied = false };
                return Ok(sp);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }

    }
}
