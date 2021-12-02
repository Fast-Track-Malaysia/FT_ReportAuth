using FT_SpReport.CoreBusiness.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public interface ISpModelService
    {
        Task<SpModel[]> getAsyncModels();
        Task<SpModel[]> getAsyncModels(string username);
        Task<SpModel> getAsyncModels(int id);
        Task<ICollection<SpParamModel>> getAsyncModelDetails(int modelid);
        Task<SpModel> updateAsyncModel(SpModel order);
        Task<SpModel> addAsyncModel(SpModel order);
    }
}