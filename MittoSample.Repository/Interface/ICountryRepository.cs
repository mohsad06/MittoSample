using MittoSample.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Repository
{
    /// <summary>
    /// The interface responsible for specifying data-related operations on Country and providing access to those operations to ServiceInterface layer.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Returns a unique Country by CountryCode
        /// </summary>
        /// <param name="code">The CountryCode field of the Country</param>
        /// <returns>A unique Country record</returns>
        Task<Country> GetByCodeAsync(string code);

        /// <summary>
        /// Returns a unique Country by MobileCountryCode
        /// </summary>
        /// <param name="mobileCode">The MobileCountryCode field of the Country</param>
        /// <returns>A unique Country record</returns>
        Task<Country> GetByMobileCodeAsync(string mobileCode);

        /// <summary>
        /// Returns all Country records in the database
        /// </summary>
        /// <returns>A list of all Country records</returns>
        Task<List<Country>> GetAllAsync();
    }
}
