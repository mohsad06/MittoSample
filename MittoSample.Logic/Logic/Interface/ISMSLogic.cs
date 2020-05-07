using MittoSample.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic
{
    /// <summary>
    /// The interface responsible for specifying logic-related operations on SMS and providing access to those operations to ServiceInterface layer.
    /// </summary>
    public interface ISMSLogic
    {
        /// <summary>
        /// Adds a SMS, the MobileCountryCode will be set and SMS will be sent to the receiver's phone number.
        /// </summary>
        /// <param name="receiverNumber">The phone number of the receiver of SMS</param>
        /// <param name="senderNumber">The phone number of the sender of SMS</param>
        /// <param name="text">The text of SMS</param>
        Task AddAsync(string receiverNumber, string senderNumber, string text);

        /// <summary>
        /// Filters exisiting SMS by date range and with skipping and taking the specified amount.
        /// </summary>
        /// <param name="fromDate">The start of the date range</param>
        /// <param name="toDate">The end of date range</param>
        /// <param name="skip">The number of records to be skipped</param>
        /// <param name="take">The number of records to be taken</param>
        /// <returns>A list of filtered SMS</returns>
        Task<IEnumerable<SMS>> FilterByDateAsync(DateTime fromDate, DateTime toDate, int skip, int take);

        /// <summary>
        /// Aggregates exisiting SMS by country and send date then filteres result by the specified date range and list of countries
        /// </summary>
        /// <param name="fromDate">The start of date range</param>
        /// <param name="toDate">The end of date range</param>
        /// <param name="countries">The list of countries to be aggregated upon, when empty all existing countries are considered</param>
        /// <returns>A list of aggregated SMS filtered by date range and countries</returns>
        Task<IEnumerable<SMSGroupBy>> FilterCountryDayAsync(DateTime fromDate, DateTime toDate, string countries);
    }
}
