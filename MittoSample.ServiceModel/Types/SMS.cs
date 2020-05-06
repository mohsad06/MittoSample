using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MittoSample.ServiceModel.Types
{
    public class SMS
    {
        [AutoId]
        public Guid Id { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Receiver { get; set; }
        [StringLength(StringLengthAttribute.MaxText)]
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public string MobileCountryCode { get; set; }
    }
}
