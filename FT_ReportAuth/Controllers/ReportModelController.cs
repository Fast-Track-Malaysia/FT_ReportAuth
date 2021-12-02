﻿using Dapper;
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
    public class ReportModelController : ControllerBase
    {
        public ReportModelController(ApplicationDbContext myContext, IConfiguration configuration)
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
                var sp = MyContext.ReportModels.ToArray();

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
                var sp = MyContext.ReportModels.Where<ReportModel>(pp => pp.Id == id).FirstOrDefault();
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
        [HttpPost("{id:int}")]
        public IActionResult CustomPost(int id, [FromBody] ReportModel json)
        {
            try
            {
                string rtn = "No records update.";
                var sp = MyContext.ReportModels.Where<ReportModel>(pp => pp.Id == id).FirstOrDefault();
                if (sp == null)
                {
                    return Problem(rtn);
                }

                MyContext.Entry(sp).CurrentValues.SetValues(json);
                MyContext.SaveChanges();

                sp = MyContext.ReportModels.Where<ReportModel>(pp => pp.Id == id).FirstOrDefault();
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
        public IActionResult CustomPut([FromBody] ReportModel json)
        {
            try
            {
                string rtn = "No record added.";

                var sp = MyContext.ReportModels.Where<ReportModel>(pp => pp.ReportName == json.ReportName).FirstOrDefault();
                if (sp != null)
                {
                    return Problem("Record Found");
                }

                MyContext.ReportModels.Add(json);
                MyContext.SaveChanges();

                sp = MyContext.ReportModels.Where<ReportModel>(pp => pp.Id == json.Id).FirstOrDefault();
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
