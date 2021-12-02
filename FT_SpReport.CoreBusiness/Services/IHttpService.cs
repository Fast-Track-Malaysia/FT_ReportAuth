using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri);
        Task Login(string username, string password);
        Task<T> Post<T>(string uri, object value);
        Task<T> Put<T>(string uri, object value);
        Task<object> PostReturnStream(string uri, object value);
    }
}