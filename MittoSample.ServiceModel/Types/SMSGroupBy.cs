﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MittoSample.ServiceModel.Types
{
    public class SMSGroupBy
    {
        public string MobileCountryCode { get; set; }
        public DateTime SendDate { get; set; }
        public int Count { get; set; }
    }
}
