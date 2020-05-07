using ServiceStack.DataAnnotations;
using System;

namespace MittoSample.Model
{
    /// <summary>
    /// The POCO class for County model, responsible for mapping data to database
    /// </summary>
    public class Country
    {
        [AutoId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Unique]
        public string MobileCountryCode { get; set; }
        [Unique]
        public string CountryCode { get; set; }
        public decimal PricePerSMS { get; set; }
    }
}
