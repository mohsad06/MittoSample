using MittoSample.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic
{
    public interface ISMSLogic
    {
        Task AddAsync(string receiverNumber, string senderNumber, string text);

        Task<IEnumerable<SMS>> FilterAllAsync(DateTime fromDate, DateTime toDate, int skip, int take);

        Task<IEnumerable<SMSGroupBy>> FilterCountryDayAsync(DateTime fromDate, DateTime toDate, string Countries);
    }
}
