﻿using System;

namespace MittoSample.Model
{
    /// <summary>
    /// The POCO class created for mapping data generated by aggregation on SMS
    /// </summary>
    public class SMSGroupBy
    {
        public string MobileCountryCode { get; set; }
        public DateTime SendDate { get; set; }
        public int Count { get; set; }
    }
}
