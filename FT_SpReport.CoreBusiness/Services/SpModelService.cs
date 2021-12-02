using FT_SpReport.CoreBusiness.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public class SpModelService : ISpModelService
    {
        private IHttpService _httpService;
        //private ISpDBContext _spDBContext { get; set; }
        public SpModelService(
            IHttpService httpService
            )
        {
            this._httpService = httpService;
        }
        public async Task<SpModel[]> getAsyncModels()
        {
            //return await Task.FromResult(_spDBContext.SpModels.ToArray());
            return await _httpService.Get<SpModel[]>("api/spmodel/");
        }
        public async Task<SpModel[]> getAsyncModels(string username)
        {
            //return await Task.FromResult(_spDBContext.SpModels.ToArray());
            return await _httpService.Get<SpModel[]>("api/spmodelfilter/" + username);
        }
        public async Task<SpModel> getAsyncModels(int id)
        {
            //return await Task.FromResult(_spDBContext.SpModels.Where(pp => pp.Id == id).FirstOrDefault());
            return await _httpService.Get<SpModel>("api/spmodel/" + id.ToString());
        }
        public async Task<ICollection<SpParamModel>> getAsyncModelDetails(int modelid)
        {
            //return await Task.FromResult(_spDBContext.SpParamModels.Where(pp => pp.SpModel.Id == modelid).ToList());
            return await _httpService.Get<ICollection<SpParamModel>>("api/spparammodel/" + modelid.ToString());
        }
        public async Task<SpModel> updateAsyncModel(SpModel order)
        {
            return await _httpService.Post<SpModel>("api/spmodel/" + order.Id.ToString(), order);
        }
        public async Task<SpModel> addAsyncModel(SpModel order)
        {
            return await _httpService.Post<SpModel>("api/spmodel", order);
        }
    }
}
