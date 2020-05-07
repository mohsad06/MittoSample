using MittoSample.Logic.Repository;
using MittoSample.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic
{
    public class CountryLogic : ICountryLogic
    {
        private ICountryRepository _countryRepository { get; set; }

        public CountryLogic(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            IEnumerable<Country> result = await _countryRepository.GetAllAsync();
            return result;
        }

        public Task<Country> GetByCodeAsync(string code)
        {
            return _countryRepository.GetByCodeAsync(code);
        }

        public Task<Country> GetByMobileCodeAsync(string mobileCode)
        {
            return _countryRepository.GetByMobileCodeAsync(mobileCode);
        }
    }
}
