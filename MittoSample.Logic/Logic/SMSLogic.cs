﻿using MittoSample.Logic.Repository;
using MittoSample.Model;
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

        public async Task AddAsync(string receiverNumber, string senderNumber, string text)
        {
            var sms = new SMS { Receiver = receiverNumber, Sender = senderNumber, Text = text };

            await ExtractMobileCountryCode(sms);

            await _smsRepository.AddAsync(sms);

            SendSMSDummyImplementation(sms);
        }

        public async Task<IEnumerable<SMS>> FilterByDateAsync(DateTime fromDate, DateTime toDate, int skip, int take)
        {
            IEnumerable<SMS> result = await _smsRepository.FilterByDateAsync(fromDate, toDate, skip, take);

            return result;
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
            IEnumerable<SMSGroupBy> result = await _smsRepository.GroupByCountryDayAsync(fromDate, toDate, countries);

            return result;
        }
    }
}
