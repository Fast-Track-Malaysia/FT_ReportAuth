using FT_SpReport.CoreBusiness.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public interface ISpReportService
    {
        Task<T> postAsyncModel<T>(string spname, string username, SpParamModel[] order);
        Task<object> postAsyncModelExport(string spname, string username, SpParamModel[] order);
        Task<T> postAsyncModel<T>(string spname, string username);
    }
}