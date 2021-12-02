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
    public class AssignRoleController : ControllerBase
    {
        public AssignRoleController(ApplicationDbContext myContext, IConfiguration configuration)
        {
            MyContext = myContext;
            Configuration = configuration;
        }
        private ApplicationDbContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpPost("{type}/{name}")]
        public IActionResult CustomGet(string type, string name, [FromBody] SpRole[] roles)
        {
            try
            {
                string rtn = "Update failed.";
                object obj = null;
                if (type == SpRoleypeEnum.user.ToString())
                {
                    obj = MyContext.Users.Where<IdentityUser>(pp => pp.UserName == name).FirstOrDefault();
                    if (obj == null)
                        return Problem(rtn);
                }
                else if (type == SpRoleypeEnum.spmodel.ToString())
                {
                    obj = MyContext.SpModels.Where<SpModel>(pp => pp.SpName == name).FirstOrDefault();
                    if (obj == null)
                        return Problem(rtn);
                }
                else if (type == SpRoleypeEnum.reportmodel.ToString())
                {
                    obj = MyContext.ReportModels.Where<ReportModel>(pp => pp.ReportName == name).FirstOrDefault();
                    if (obj == null)
                        return Problem(rtn);
                }

                foreach (var item in roles)
                {
                    var role = MyContext.Roles.Where<IdentityRole>(pp => pp.Name == item.Name).FirstOrDefault();
                    if (item.Applied)
                    {
                        if (type == SpRoleypeEnum.user.ToString())
                        {
                            var data = obj as IdentityUser;
                            var find = MyContext.UserRoles.Where(pp => pp.UserId == data.Id.ToString() && pp.RoleId == role.Id.ToString()).FirstOrDefault();
                            if (find == null)
                            {
                                find = new IdentityUserRole<string>() { UserId = data.Id.ToString(), RoleId = role.Id.ToString() };
                                MyContext.UserRoles.Add(find);
                                MyContext.SaveChanges();
                            }
                        }
                        else if (type == SpRoleypeEnum.spmodel.ToString())
                        {
                            var data = obj as SpModel;
                            var find = MyContext.SpModelRoles.Where<SpModelRole>(pp => pp.SpModel == data && pp.UserRole == role).FirstOrDefault();
                            if (find == null)
                            {
                                find = new SpModelRole() { SpModel = data, UserRole = role };
                                MyContext.SpModelRoles.Add(find);
                                MyContext.SaveChanges();
                            }
                        }
                        else if (type == SpRoleypeEnum.reportmodel.ToString())
                        {
                            var data = obj as ReportModel;
                            var find = MyContext.ReportModelRoles.Where<ReportModelRole>(pp => pp.SpModel == data && pp.UserRole == role).FirstOrDefault();
                            if (find == null)
                            {
                                find = new ReportModelRole() { SpModel = data, UserRole = role };
                                MyContext.ReportModelRoles.Add(find);
                                MyContext.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        if (type == SpRoleypeEnum.user.ToString())
                        {
                            var data = obj as IdentityUser;
                            var find = MyContext.UserRoles.Where(pp => pp.UserId == data.Id.ToString() && pp.RoleId == role.Id.ToString()).FirstOrDefault();
                            if (find != null)
                            {
                                MyContext.UserRoles.Remove(find);
                                MyContext.SaveChanges();
                            }
                        }
                        else if (type == SpRoleypeEnum.spmodel.ToString())
                        {
                            var data = obj as SpModel;
                            var find = MyContext.SpModelRoles.Where<SpModelRole>(pp => pp.SpModel == data && pp.UserRole == role).FirstOrDefault();
                            if (find != null)
                            {
                                MyContext.SpModelRoles.Remove(find);
                                MyContext.SaveChanges();
                            }
                        }
                        else if (type == SpRoleypeEnum.reportmodel.ToString())
                        {
                            var data = obj as ReportModel;
                            var find = MyContext.ReportModelRoles.Where<ReportModelRole>(pp => pp.SpModel == data && pp.UserRole == role).FirstOrDefault();
                            if (find != null)
                            {
                                MyContext.ReportModelRoles.Remove(find);
                                MyContext.SaveChanges();
                            }
                        }

                    }
                }
                if (type == SpRoleypeEnum.user.ToString())
                    return Ok(obj as IdentityUser);
                else if (type == SpRoleypeEnum.spmodel.ToString())
                    return Ok(obj as SpModel);
                else if (type == SpRoleypeEnum.reportmodel.ToString())
                    return Ok(obj as ReportModel);
                else
                    return null;

            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }

    }
}
