using Dapper;
using FT_SpReport.CoreBusiness.Models;
using FT_SpReport.CoreBusiness.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FT_ReportAuth.Services;
using System.Dynamic;
using FT_SpReport.CoreBusiness.Helpers;
using FT_ReportAuth.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace FT_ReportAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupResultController : ControllerBase
    {
        public LookupResultController(IConfiguration configuration, ApplicationDbContext spDBContext)
        {
            Configuration = configuration;
            MyContext = spDBContext;

        }
        private ApplicationDbContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpGet("{lookupname}")]
        public IActionResult CustomPost(string lookupname)
        {
            List<LookupResult> rtn = new List<LookupResult>();

            try
            {
                //username = MyContext.Users.Where(pp => pp.UserName == username).Select(pp => pp.Id).FirstOrDefault().ToString();

                var sp = MyContext.LookupModels.Where<LookupModel>(pp => pp.LookUpName == lookupname).FirstOrDefault();
                if (sp == null)
                {
                    rtn.Add(new LookupResult() { Code = "", Name = "No record found." });
                    return Ok(rtn);
                }
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    rtn = conn.Query<LookupResult>(sp.LookUpSQL).ToList();
                    if (rtn.Count > 0)
                    {
                        return Ok(rtn);
                    }
                }
                //temp = string.Format("{0} No record found.", spname);
                //WriteLog("Not Found", portaluserid, temp);
                rtn.Add(new LookupResult() { Code = "", Name = "No record found." });
                return Ok(rtn);
            }
            catch (Exception ex)
            {
                //return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
                //rtn = new ExpandoObject();
                rtn.Add(new LookupResult() { Code = "", Name = ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message) });
                return Ok(rtn);
            }
        }

        [HttpGet("{spname}/{username}")]
        public IActionResult CustomPost(string spname, string username)
        {
            string rtn = "No query found.";
            try
            {
                var sp = MyContext.SpDatas.Where<SpData>(pp => pp.SpName == spname && pp.User.UserName == username).FirstOrDefault();

                if (sp != null && sp.JsonData != null && !string.IsNullOrWhiteSpace(sp.JsonData))
                {
                    string js = sp.JsonData;
                    //List<ExpandoObject> loadedData = JsonConvert.DeserializeObject<List<ExpandoObject>>(js);
                    //DataTable dt = loadedData.ToDataTable("table");
                    return Ok(js);
                }
                //temp = string.Format("{0} No record found.", spname);
                //WriteLog("Not Found", portaluserid, temp);
                return Problem(rtn);
            }
            catch (Exception ex)
            {
                //return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
                rtn = ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message);
                return Problem(rtn);
            }
        }
    }
}
