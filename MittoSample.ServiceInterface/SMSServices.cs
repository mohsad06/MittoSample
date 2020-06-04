using MittoSample.Model;
using MittoSample.Repository;
using MittoSample.ServiceModel;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MittoSample.ServiceInterface
{
    /// <summary>
    /// The service class responsible for handling SMSDto requests by usage of Repository layer.
    /// </summary>
    public class SMSServices : Service
    {
        private ISMSRepository _smsRepository { get; set; }
        private ICountryRepository _countryRepository { get; set; }

        public SMSServices(ISMSRepository smsRepository, ICountryRepository countryRepository)
        {
            _smsRepository = smsRepository;
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Service method responsible for handling HTTP Get verb of SendSMS DTO request
        /// </summary>
        /// <param name="request">an object of SendSMS DTO type</param>
        /// <returns>an object of SendSMSResponse DTO type</returns>
        public async Task<SendSMSResponse> Get(SendSMS request)
        {
            //Create SMS object
            var sms = new SMS
            {
                Receiver = request.To,
                Sender = request.From,
                Text = request.Text
            };

            //Extract and set the MobileCountryCode based on the phone number of the receiver
            await ExtractMobileCountryCode(sms);

            //Calling database insert operation
            await _smsRepository.AddAsync(sms);

            //Now that the insert operation is complete, we'll use the following method to actually send the SMS to the receiver
            SendSMSDummyImplementation(sms);

            return new SendSMSResponse { State = StateEnum.Success };
        }

        /// <summary>
        /// Searches for a Country with the CountryCode of the SMS receiver phone number and sets the MobileCountryCode for the SMS object
        /// </summary>
        /// <param name="sms">The SMS object containing receiver's phone number</param>
        private async Task ExtractMobileCountryCode(SMS sms)
        {
            var countryCode = sms.Receiver?.TrimStart('+', '0')?.Substring(0, 2);

            if (!string.IsNullOrEmpty(countryCode))
            {
                var country = await _countryRepository.GetByCodeAsync(countryCode);
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

        /// <summary>
        /// Service method responsible for handling HTTP Get verb of GetSentSMS DTO request
        /// </summary>
        /// <param name="request">an object of GetSentSMS DTO type</param>
        /// <returns>an object of GetSentSMSResponse DTO type</returns>
        public async Task<GetSentSMSResponse> Get(GetSentSMS request)
        {
            var filteredList = await _smsRepository.FilterByDateAsync(
                DateTime.Parse(request.DateTimeFrom),
                DateTime.Parse(request.DateTimeTo),
                request.Skip,
                request.Take);

            var sentSMSResponses = new List<SentSMSResponse>();

            foreach (var item in filteredList)
            {
                var itemCountry = (await _countryRepository.GetByMobileCodeAsync(item.MobileCountryCode));

                decimal itemPrice = itemCountry != null ? itemCountry.PricePerSMS : 0;

                sentSMSResponses.Add(new SentSMSResponse
                {
                    DateTime = item.SendDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Mcc = item.MobileCountryCode,
                    From = item.Sender,
                    To = item.Receiver,
                    Price = decimal.Parse(itemPrice.ToString().TrimEnd('0').TrimEnd('.'))
                });
            }

            return new GetSentSMSResponse
            {
                TotalCount = sentSMSResponses.Count,
                Items = sentSMSResponses,
                State = StateEnum.Success
            };
        }

        /// <summary>
        /// Service method responsible for handling HTTP Get verb of GetStatistics DTO request
        /// </summary>
        /// <param name="request">an object of GetStatistics DTO type</param>
        /// <returns>an object of GetStatisticsResponse DTO type</returns>
        public async Task<GetStatisticsResponse> Get(GetStatistics request)
        {
            var filteredSMSList = await _smsRepository.GroupByCountryDayAsync(
                DateTime.Parse(request.DateFrom),
                DateTime.Parse(request.DateTo),
                request.MccList);

            var statisticsResponses = new List<StatisticsResponse>();

            foreach (var item in filteredSMSList)
            {
                var itemCountry = await _countryRepository.GetByMobileCodeAsync(item.MobileCountryCode);

                statisticsResponses.Add(new StatisticsResponse
                {
                    Count = item.Count,
                    Day = item.SendDate.Date.ToString("yyyy-MM-dd"),
                    Mcc = item.MobileCountryCode,
                    PricePerSMS = decimal.Parse(itemCountry.PricePerSMS.ToString().TrimEnd('0').TrimEnd('.')),
                    TotalPrice = decimal.Parse((item.Count * itemCountry.PricePerSMS).ToString().TrimEnd('0').TrimEnd('.'))
                });
            }

            return new GetStatisticsResponse { Items = statisticsResponses };
        }
    }
}
