using MittoSample.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic
{
    /// <summary>
    /// The interface responsible for specifying logic-related operations on Country and providing access to those operations to ServiceInterface layer.
    /// </summary>
    public interface ICountryLogic
    {
        /// <summary>
        /// Returns a unique Country
        /// </summary>
        /// <param name="code">The CountryCode of the Country</param>
        /// <returns>A uniqye Country</returns>
        Task<Country> GetByCodeAsync(string code);

        /// <summary>
        /// Returns a unique Country
        /// </summary>
        /// <param name="mobileCode">The MobileCountryCode of the Country</param>
        /// <returns>A uniqye Country</returns>
        Task<Country> GetByMobileCodeAsync(string mobileCode);

        /// <summary>
        /// Returns all Country records
        /// </summary>
        /// <returns>A list of all Country records</returns>
        Task<IEnumerable<Country>> GetAllAsync();
    }
}
