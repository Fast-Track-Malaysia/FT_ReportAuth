using FT_SpReport.CoreBusiness.Models;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public interface IReportModelService
    {
        Task<ReportModel> addAsyncModel(ReportModel order);
        Task<ReportModel[]> getAsyncModels();
        Task<ReportModel> getAsyncModels(int id);
        Task<ReportModel[]> getAsyncModels(string lookupname);
        Task<ReportModel> updateAsyncModel(ReportModel order);
        Task<ReportModel[]> getAsyncFilterModels(string username);
    }
}