using FT_SpReport.CoreBusiness.Models;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public interface ILookupModelService
    {
        Task<LookupModel> addAsyncModel(LookupModel order);
        Task<LookupModel[]> getAsyncModels();
        Task<LookupModel> getAsyncModels(int id);
        Task<LookupModel> updateAsyncModel(LookupModel order);
        Task<LookupResult[]> getAsyncModels(string lookupname);
    }
}