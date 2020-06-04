using MittoSample.Repository;
using MittoSample.ServiceModel;
using ServiceStack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.ServiceInterface
{
    /// <summary>
    /// The service class responsible for handling CountryDto requests by usage of Repository layer.
    /// </summary>
    public class CountryServices : Service
    {
        private ICountryRepository _countryRepository { get; set; }

        public CountryServices(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Service method responsible for handling HTTP Get verb of GetCountries DTO request
        /// </summary>
        /// <param name="request">an object of GetCountries DTO type</param>
        /// <returns>an object of GetCountriesResponse DTO type</returns>
        public async Task<GetCountriesResponse> Get(GetCountries request)
        {
            var countryList = await _countryRepository.GetAllAsync();

            var countryResponses = new List<CountryResponse>();

            foreach (var item in countryList)
            {
                countryResponses.Add(new CountryResponse
                {
                    Mcc = item.MobileCountryCode,
                    Cc = item.CountryCode,
                    Name = item.Name,
                    PricePerSMS = decimal.Parse(item.PricePerSMS.ToString().TrimEnd('0').TrimEnd('.'))
                });
            }

            return new GetCountriesResponse { Result = countryResponses };
        }
    }
}
