using ServiceStack;
using System.Collections.Generic;

namespace MittoSample.ServiceModel
{
    /// <summary>
    /// GetCountries request DTO class
    /// </summary>
    [Route("/countries")]
    public class GetCountries : IReturn<GetCountriesResponse>
    {
    }

    /// <summary>
    /// GetCountries response DTO class.
    /// </summary>
    public class GetCountriesResponse
    {
        /// <summary>
        /// A DTO mapped list of countries
        /// </summary>
        public List<CountryResponse> Result { get; set; }

        /// <summary>
        /// All exceptions get injected into this property.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// Used in GetCountries response DTO class to generate a list of results.
    /// </summary>
    public class CountryResponse
    {
        /// <summary>
        /// Mapped to MobileCountryCode of the Country class
        /// </summary>
        public string Mcc { get; set; }
        /// <summary>
        /// Mapped to CountryCode of the Country class
        /// </summary>
        public string Cc { get; set; }
        /// <summary>
        /// Mapped to Name of the Country class
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mapped to PricePerSMS of the Country class
        /// </summary>
        public decimal PricePerSMS { get; set; }
    }
}
