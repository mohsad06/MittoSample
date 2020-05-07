using MittoSample.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic.Repository
{
    public interface ISMSRepository
    {
        Task AddAsync(SMS sms);

        Task<List<SMS>> GetAllAsync();

        Task<List<SMSGroupBy>> GroupByCountryDayAsync();
    }
}
