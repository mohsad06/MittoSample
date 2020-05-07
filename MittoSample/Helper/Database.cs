using MittoSample.Model;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MittoSample.Helper
{
    internal class Database
    {
        private static IDbConnectionFactory _dbConnectionFactory { get; set; }

        public Database(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public static void Create()
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                if (!db.TableExists<SMS>())
                    db.CreateTable<SMS>();

                if (!db.TableExists<Country>())
                {
                    db.CreateTable<Country>();

                    #region AddDefaultCountries

                    db.Insert(new Country { Name = "Germany", CountryCode = "49", MobileCountryCode = "262", PricePerSMS = 0.055m });
                    db.Insert(new Country { Name = "Austria", CountryCode = "43", MobileCountryCode = "232", PricePerSMS = 0.053m });
                    db.Insert(new Country { Name = "Poland", CountryCode = "48", MobileCountryCode = "260", PricePerSMS = 0.032m });

                    #endregion
                }
            }
        } 
    }
}
