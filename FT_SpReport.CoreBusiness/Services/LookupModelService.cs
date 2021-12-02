using FT_SpReport.CoreBusiness.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public class LookupModelService : ILookupModelService
    {
        private IHttpService _httpService;
        //private ISpDBContext _spDBContext { get; set; }
        public LookupModelService(
            IHttpService httpService
            )
        {
            this._httpService = httpService;
        }
        public async Task<LookupModel[]> getAsyncModels()
        {
            //return await Task.FromResult(_spDBContext.SpModels.ToArray());
            return await _httpService.Get<LookupModel[]>("api/lookupmodel/");
        }
        public async Task<LookupModel> getAsyncModels(int id)
        {
            //return await Task.FromResult(_spDBContext.SpModels.Where(pp => pp.Id == id).FirstOrDefault());
            return await _httpService.Get<LookupModel>("api/lookupmodel/" + id.ToString());
        }
        public async Task<LookupModel> updateAsyncModel(LookupModel order)
        {
            return await _httpService.Post<LookupModel>("api/lookupmodel/" + order.Id.ToString(), order);
        }
        public async Task<LookupModel> addAsyncModel(LookupModel order)
        {
            return await _httpService.Post<LookupModel>("api/lookupmodel", order);
        }
        public async Task<LookupResult[]> getAsyncModels(string lookupname)
        {
            //return await Task.FromResult(_spDBContext.SpModels.ToArray());
            return await _httpService.Get<LookupResult[]>("api/lookupresult/" + lookupname);
        }
    }
}
