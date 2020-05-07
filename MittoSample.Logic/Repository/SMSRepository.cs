using MittoSample.Model;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.Logic.Repository
{
    public class SMSRepository : ISMSRepository
    {
        private IDbConnectionFactory _dbConnectionFactory { get; set; }

        public SMSRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task AddAsync(SMS sms)
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                sms.SendDate = DateTime.UtcNow;
                await db.InsertAsync(sms);
            }
        }

        public Task<List<SMS>> GetAllAsync()
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                return db.SelectAsync<SMS>();
            }
        }

        public Task<List<SMSGroupBy>> GroupByCountryDayAsync()
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                return db.SelectAsync<SMSGroupBy>(@"
                            SELECT MobileCountryCode, DATE(SendDate), Count(*)
                            FROM mittosample.sms
                            GROUP BY MobileCountryCode, DATE(SendDate)");
            }
        }
    }
}
