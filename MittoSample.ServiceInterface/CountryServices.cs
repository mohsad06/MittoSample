using MittoSample.Logic;
using MittoSample.ServiceModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MittoSample.ServiceInterface
{
    public class CountryServices : Service
    {
        private ICountryLogic _countryLogic { get; set; }

        public CountryServices(ICountryLogic countryLogic)
        {
            _countryLogic = countryLogic;
        }

        public async Task<GetCountriesResponse> Get(GetCountries request)
        {
            var countryList = await _countryLogic.GetAllAsync();

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
