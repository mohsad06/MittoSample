using MittoSample.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic.Repository
{
    public interface ICountryRepository
    {
        Task<Country> GetByCodeAsync(string code);

        Task<Country> GetByMobileCodeAsync(string mobileCode);

        Task<List<Country>> GetAllAsync();
    }
}
