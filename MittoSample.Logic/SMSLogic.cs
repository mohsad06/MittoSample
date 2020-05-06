using MittoSample.Logic.Repository;
using MittoSample.ServiceModel.Types;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MittoSample.Logic
{
    public class SMSLogic : ISMSLogic
    {
        private ISMSRepository _smsRepository { get; set; }
        private ICountryLogic _countryLogic { get; set; }

        public SMSLogic(ISMSRepository smsRepostiory, ICountryLogic countryLogic)
        {
            _smsRepository = smsRepostiory;
            _countryLogic = countryLogic;
        }

        public async Task AddAsync(SMS sms)
        {
            await ExtractMobileCountryCode(sms);

            await _smsRepository.AddAsync(sms);

            SendSMSDummyImplementation(sms);
        }

        public async Task<IEnumerable<SMS>> FilterAllAsync(DateTime fromDate, DateTime toDate, int skip, int take)
        {
            IEnumerable<SMS> result = await _smsRepository.GetAllAsync();

            return result.Where(x => x.SendDate >= fromDate && x.SendDate <= toDate).Skip(skip).Take(take);
        }

        private async Task ExtractMobileCountryCode(SMS sms)
        {
            var countryCode = sms.Receiver?.TrimStart('+', '0')?.Substring(0, 2);

            if (!string.IsNullOrEmpty(countryCode))
            {
                var country = await _countryLogic.GetByCodeAsync(countryCode);
                sms.MobileCountryCode = country.MobileCountryCode;
            }
        }

        //TODO: Replace with the actual logic of sending a message
        private bool SendSMSDummyImplementation(SMS sms)
        {
            var dataToLog = JsonSerializer.SerializeToString(sms);
            var logDirectoryPath = Environment.CurrentDirectory + @"\Log";

            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }

            var logFilePath = logDirectoryPath + @"\SMSLog.txt";
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath);
            }

            using (var tw = new StreamWriter(logFilePath, true))
            {
                tw.WriteLine(dataToLog);
            }

            return true;
        }

        public async Task<IEnumerable<SMSGroupBy>> FilterCountryDayAsync(DateTime fromDate, DateTime toDate, string countries)
        {
            IEnumerable<SMSGroupBy> result = await _smsRepository.GroupByCountryDayAsync();

            result = result.Where(x => x.SendDate.Date >= fromDate.Date && x.SendDate.Date <= toDate.Date);

            if (!string.IsNullOrEmpty(countries))
            {
                var countryList = countries.Split(',');
                result = result.Where(x => countryList.Contains(x.MobileCountryCode));
            }
            return result;
        }
    }
}
