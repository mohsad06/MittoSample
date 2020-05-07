using MittoSample.Model;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MittoSample.Helper
{
    /// <summary>
    /// Responsible for creating database and adding default data to database tables.
    /// </summary>
    internal class Database
    {
        private static IDbConnectionFactory _dbConnectionFactory { get; set; }

        public Database(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// Creates database schema.
        /// </summary>
        public static void Create()
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                if (!db.TableExists<SMS>())
                    db.CreateTable<SMS>();

                if (!db.TableExists<Country>())
                    db.CreateTable<Country>();
            }
        }

        /// <summary>
        /// Inserts default data to database tables
        /// </summary>
        public static void AddDefaultData()
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                db.Insert(new Country { Name = "Germany", CountryCode = "49", MobileCountryCode = "262", PricePerSMS = 0.055m });
                db.Insert(new Country { Name = "Austria", CountryCode = "43", MobileCountryCode = "232", PricePerSMS = 0.053m });
                db.Insert(new Country { Name = "Poland", CountryCode = "48", MobileCountryCode = "260", PricePerSMS = 0.032m });
            }
        }
    }
}
