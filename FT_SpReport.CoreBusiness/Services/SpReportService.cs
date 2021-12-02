using FT_SpReport.CoreBusiness.Helpers;
using FT_SpReport.CoreBusiness.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public class SpReportService : ISpReportService
    {
        private IHttpService _httpService;
        private ILocalStorageService _localStorageService;
        //private ISpDBContext _spDBContext { get; set; }
        public SpReportService(
            IHttpService httpService,
            ILocalStorageService localStorageService
            )
        {
            this._httpService = httpService;
            this._localStorageService = localStorageService;
        }
        public async Task<T> postAsyncModel<T>(string spname, string username, SpParamModel[] order)
        {
            return await _httpService.Post<T>("api/SpReport/" + spname + "/" + username, order);
        }
        public async Task<object> postAsyncModelExport(string spname, string username, SpParamModel[] order)
        {
            return await _httpService.PostReturnStream("api/SpExport/" + spname + "/" + username, order);
        }
        public async Task<T> postAsyncModel<T>(string spname, string username)
        {
            return await _httpService.Get<T>("api/SpReport/" + spname + "/" + username);
        }
    }
}
