using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MittoSample.ServiceModel
{
    [Route("/countries")]
    public class GetCountries : IReturn<GetCountriesResponse>
    {
    }

    public class GetCountriesResponse
    {
        public List<CountryResponse> Result { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class CountryResponse
    {
        public string Mcc { get; set; }
        public string Cc { get; set; }
        public string Name { get; set; }
        public decimal PricePerSMS { get; set; }
    }
}
