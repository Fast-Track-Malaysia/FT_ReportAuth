using FT_SpReport.CoreBusiness.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public class ReportModelService : IReportModelService
    {
        private IHttpService _httpService;
        //private ISpDBContext _spDBContext { get; set; }
        public ReportModelService(
            IHttpService httpService
            )
        {
            this._httpService = httpService;
        }
        public async Task<ReportModel[]> getAsyncModels()
        {
            //return await Task.FromResult(_spDBContext.SpModels.ToArray());
            return await _httpService.Get<ReportModel[]>("api/reportmodel/");
        }
        public async Task<ReportModel> getAsyncModels(int id)
        {
            //return await Task.FromResult(_spDBContext.SpModels.Where(pp => pp.Id == id).FirstOrDefault());
            return await _httpService.Get<ReportModel>("api/reportmodel/" + id.ToString());
        }
        public async Task<ReportModel> updateAsyncModel(ReportModel order)
        {
            return await _httpService.Post<ReportModel>("api/reportmodel/" + order.Id.ToString(), order);
        }
        public async Task<ReportModel> addAsyncModel(ReportModel order)
        {
            return await _httpService.Post<ReportModel>("api/reportmodel", order);
        }
        public async Task<ReportModel[]> getAsyncModels(string lookupname)
        {
            //return await Task.FromResult(_spDBContext.SpModels.ToArray());
            return await _httpService.Get<ReportModel[]>("api/reportmodel/" + lookupname);
        }

        public async Task<ReportModel[]> getAsyncFilterModels(string username)
        {
            //return await Task.FromResult(_spDBContext.SpModels.ToArray());
            return await _httpService.Get<ReportModel[]>("api/reportmodelfilter/" + username);
        }

    }
}
