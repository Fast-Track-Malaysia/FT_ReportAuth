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

namespace FT_ReportAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpModelController : ControllerBase
    {
        public SpModelController(ApplicationDbContext myContext, IConfiguration configuration)
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
                var sp = MyContext.SpModels.ToArray();

                //return Ok(JsonConvert.SerializeObject(sp));
                return Ok(sp);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }
        [HttpGet("{id:int}")]
        public IActionResult CustomGet(int id)
        {
            try
            {
                string rtn = "No records found.";
                var sp = MyContext.SpModels.Where<SpModel>(pp => pp.Id == id).FirstOrDefault();
                if (sp == null)
                {
                    return Problem(rtn);
                }
                sp.Details = MyContext.SpParamModels.Where<SpParamModel>(pp => pp.SpModel == sp).OrderBy(pp => pp.Seq).ToList();
                //string json = JsonConvert.SerializeObject(sp, Formatting.Indented,
                //                new JsonSerializerSettings
                //                {
                //                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                //                });
                return Ok(JsonConvert.SerializeObject(sp));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }
        [HttpPost("{id:int}")]
        public IActionResult CustomPost(int id, [FromBody]SpModel json)
        {
            try
            {
                string rtn = "No records update.";
                var sp = MyContext.SpModels.Where<SpModel>(pp => pp.Id == id).FirstOrDefault();
                if (sp == null)
                {
                    return Problem(rtn);
                }
                sp.Details = MyContext.SpParamModels.Where<SpParamModel>(pp => pp.SpModel.Id == id).ToList();

                MyContext.Entry(sp).CurrentValues.SetValues(json);
                #region delete details
                foreach (SpParamModel dtl in sp.Details)
                {
                    bool found = false;
                    foreach (SpParamModel item in json.Details)
                    {
                        if (dtl.Id == item.Id) found = true;
                    }
                    if (!found) 
                        MyContext.SpParamModels.Remove(dtl);
                }
                #endregion
                #region update details
                foreach (SpParamModel dtl in sp.Details)
                {
                    foreach (SpParamModel item in json.Details)
                    {
                        if (dtl.Id == item.Id) 
                            MyContext.Entry(dtl).CurrentValues.SetValues(item);
                    }
                }
                #endregion
                #region add details
                foreach (SpParamModel item in json.Details)
                {
                    item.SpModel = sp;
                    if (item.Id == 0) 
                        MyContext.SpParamModels.Add(item);
                }
                #endregion
                MyContext.SaveChanges();

                sp = MyContext.SpModels.Where<SpModel>(pp => pp.Id == id).FirstOrDefault();
                if (sp == null)
                {
                    return Problem(rtn);
                }
                //sp.Details = MyContext.SpParamModels.Where<SpParamModel>(pp => pp.SpModel.Id == id).ToList();
                return Ok(JsonConvert.SerializeObject(sp));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }

        [HttpPost]
        public IActionResult CustomPut([FromBody] SpModel json)
        {
            try
            {
                string rtn = "No record added.";

                var sp = MyContext.SpModels.Where<SpModel>(pp => pp.SpName == json.SpName).FirstOrDefault();
                if (sp != null)
                {
                    return Problem("Record Found");
                }

                MyContext.SpModels.Add(json);
                MyContext.SaveChanges();

                sp = MyContext.SpModels.Where<SpModel>(pp => pp.Id == json.Id).FirstOrDefault();
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
