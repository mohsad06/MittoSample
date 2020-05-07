using MittoSample.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic
{
    public interface ICountryLogic
    {
        Task<Country> GetByCodeAsync(string code);

        Task<Country> GetByMobileCodeAsync(string mobileCode);

        Task<IEnumerable<Country>> GetAllAsync();
    }
}
