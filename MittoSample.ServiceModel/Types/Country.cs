using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MittoSample.ServiceModel.Types
{
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
