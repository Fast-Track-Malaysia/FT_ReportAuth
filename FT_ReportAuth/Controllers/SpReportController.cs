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
    public class SpReportController : ControllerBase
    {
        public SpReportController(IConfiguration configuration, ApplicationDbContext spDBContext)
        {
            Configuration = configuration;
            MyContext = spDBContext;

        }
        private ApplicationDbContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpPost("{spname}/{username}")]
        public IActionResult CustomPost(string spname, string username, [FromBody] SpParamModel[] json)
        {
            string col = "Result";
            List<ExpandoObject> rtn = new List<ExpandoObject>();
            ExpandoObject obj = new ExpandoObject();

            try
            {
                //username = MyContext.Users.Where(pp => pp.UserName == username).Select(pp => pp.Id).FirstOrDefault().ToString();

                var sp = MyContext.SpModels.Where<SpModel>(pp => pp.SpName == spname).FirstOrDefault();
                if (sp == null)
                {
                    GeneralHelper.AddProperty(obj, col, "Query Not Found");
                    rtn.Add(obj);
                    return Ok(rtn);
                }
                sp.Details = MyContext.SpParamModels.Where<SpParamModel>(pp => pp.SpModel == sp).OrderBy(pp => pp.Seq).ToList();


                decimal decvalue = 0;
                DateTime datevalue = DateTime.Now;
                foreach (SpParamModel param in json)
                {
                    switch (param.ParamType)
                    {
                        case SpParamTypeEnum.stringType:
                            break;
                        case SpParamTypeEnum.numberType:
                            if (!decimal.TryParse(param.ParamValue, out decvalue))
                            {
                                GeneralHelper.AddProperty(obj, col, string.Format("{0} is not a number", param.ParamName));
                                rtn.Add(obj);
                                return Ok(rtn);
                            }
                            break;
                        case SpParamTypeEnum.dateType:
                            param.ParamValue = JsonConvert.DeserializeObject<DateTime>(param.ParamValue).ToString();
                            if (!DateTime.TryParse(param.ParamValue, out datevalue))
                            {
                                GeneralHelper.AddProperty(obj, col, string.Format("{0} is not a date", param.ParamName));
                                rtn.Add(obj);
                                return Ok(rtn);
                            }
                            param.ParamValue = datevalue.ToString("yyyy-MM-dd");
                            break;
                    }
                    var dtl = sp.Details.Where<SpParamModel>(pp => pp.ParamName == param.ParamName).FirstOrDefault();
                    if (dtl != null)
                        dtl.ParamValue = param.ParamValue;
                }

                string sql = sp.SpSql;
                foreach (SpParamModel param in sp.Details)
                {
                    if (sql.Contains("{" + param.ParamName + "}"))
                    {
                        if (param.ParamType == SpParamTypeEnum.numberType)
                        {
                            sql = sql.Replace("{" + param.ParamName + "}", param.ParamValue);
                        }
                        else
                        {
                            sql = sql.Replace("{" + param.ParamName + "}", $"{param.ParamValue}");
                        }

                        continue;
                    }
                    
                    if (param.ParamType == SpParamTypeEnum.numberType)
                    {
                        sql = sql.Replace("{" + param.Seq.ToString() + "}", param.ParamValue);
                    }
                    else
                    {
                        sql = sql.Replace("{" + param.Seq.ToString() + "}", $"{param.ParamValue}");
                    }
                }
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    var list = conn.Query<dynamic>(sql).ToArray();
                    if (list.Length > 0)
                    {
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                        SpData spData = MyContext.SpDatas.Where(pp => pp.SpName == spname && pp.User.UserName == username).FirstOrDefault();
                        if (spData == null)
                        {
                            var user = MyContext.Users.Where<IdentityUser>(pp => pp.UserName == username).FirstOrDefault();
                            spData = new SpData() { SpName = spname, JsonData = jsonstr, User = user };
                            MyContext.SpDatas.Add(spData);
                            MyContext.SaveChanges();
                        }
                        else
                        {
                            spData.JsonData = jsonstr;
                            MyContext.Entry(spData).CurrentValues.SetValues(spData);
                            MyContext.SaveChanges();
                        }
                        return Ok(list);
                    }
                }
                //temp = string.Format("{0} No record found.", spname);
                //WriteLog("Not Found", portaluserid, temp);
                GeneralHelper.AddProperty(obj, col, "No record found.");
                rtn.Add(obj);
                return Ok(rtn);
            }
            catch (Exception ex)
            {
                //return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
                //rtn = new ExpandoObject();
                GeneralHelper.AddProperty(obj, col, ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
                rtn.Add(obj);
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
