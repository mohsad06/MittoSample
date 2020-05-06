using MittoSample.ServiceModel.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic
{
    public interface ISMSLogic
    {
        Task AddAsync(SMS sms);

        Task<IEnumerable<SMS>> FilterAllAsync(DateTime fromDate, DateTime toDate, int skip, int take);

        Task<IEnumerable<SMSGroupBy>> FilterCountryDayAsync(DateTime fromDate, DateTime toDate, string Countries);
    }
}
