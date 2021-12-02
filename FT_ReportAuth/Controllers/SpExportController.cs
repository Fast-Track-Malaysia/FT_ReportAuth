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
using FT_SpReport.CoreBusiness.Helpers;
using DevExpress.Blazor.Internal;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Dynamic;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using FT_ReportAuth.Data;
using FT_ReportAuth.Helpers;

namespace FT_ReportAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpExportController : ControllerBase
    {
        public SpExportController(IConfiguration configuration, ApplicationDbContext spDBContext)
        {
            Configuration = configuration;
            MyContext = spDBContext;

        }
        private ApplicationDbContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpGet]
        public async Task<IActionResult> CustomPost(string format, string spname, string username)
        {
            string rtn = "No query found." ;
            try
            {
                var sp = MyContext.SpDatas.Where<SpData>(pp => pp.SpName == spname && pp.User.UserName == username).FirstOrDefault();

                if (sp != null && sp.JsonData != null && !string.IsNullOrWhiteSpace(sp.JsonData))
                {
                    string js = sp.JsonData;
                    List<ExpandoObject> loadedData = JsonConvert.DeserializeObject<List<ExpandoObject>>(js);
                    DataTable dt = loadedData.ToDataTable("table");
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    ExportHelper helper = new ExportHelper();
                    byte[] content = await helper.ExportResultDataSet(ds, format);

                    string fileName = "DataGrid." + format;
                    string mimeType = "application/octet-stream";
                    return new FileStreamResult(new MemoryStream(content), mimeType)
                    {
                        FileDownloadName = fileName
                    };
                    //WriteLog("Log", portaluserid, _sapdoctype);
                    //string jsonstr = JsonConvert.SerializeObject(list);
                    //JsonToXML(jsonstr, _sapdoctype, true, "", "::List");
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
