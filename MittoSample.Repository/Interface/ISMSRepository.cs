using MittoSample.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Repository
{
    /// <summary>
    /// The interface responsible for specifying data-related operations on SMS and providing access to those operations to ServiceInterface layer.
    /// </summary>
    public interface ISMSRepository
    {
        /// <summary>
        /// Inserts a SMS record into the database.
        /// </summary>
        /// <param name="sms">The SMS record to be inserted</param>
        Task AddAsync(SMS sms);

        /// <summary>
        /// Returns a filtered list of SMS records in a specific date range and with skipping and taking the required amount of records.
        /// </summary>
        /// <param name="fromDate">The start of date range</param>
        /// <param name="toDate">The end of date range</param>
        /// <param name="skip">The number of records to be skipped</param>
        /// <param name="take">The number of records to be taken</param>
        /// <returns>A filtered list of SMS records</returns>
        Task<List<SMS>> FilterByDateAsync(DateTime fromDate, DateTime toDate, int skip, int take);

        /// <summary>
        /// Returns an aggregated list of SMS records by country and send date in a specific date range with skipping and taking the required amount o records.
        /// </summary>
        /// <param name="fromDate">The start of date range</param>
        /// <param name="toDate">The end of date range</param>
        /// <param name="countries">List of countries to be aggregated upon</param>
        /// <returns>A filtered list of aggregated SMS records</returns>
        Task<IEnumerable<SMSGroupBy>> GroupByCountryDayAsync(DateTime fromDate, DateTime toDate, string countries);
    }
}
