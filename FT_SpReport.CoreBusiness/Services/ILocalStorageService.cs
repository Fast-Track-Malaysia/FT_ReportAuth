using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Services
{
    public interface ILocalStorageService
    {
        Task<T> GetItem<T>(string key);
        Task RemoveItem(string key);
        Task SetItem<T>(string key, T value);
    }
}