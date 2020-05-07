using MittoSample.Logic;
using MittoSample.ServiceModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MittoSample.ServiceInterface
{
    public class SMSServices : Service
    {
        private ISMSLogic _smsLogic { get; set; }
        private ICountryLogic _countryLogic { get; set; }

        public SMSServices(ISMSLogic smsLogic, ICountryLogic countryLogic)
        {
            _smsLogic = smsLogic;
            _countryLogic = countryLogic;
        }

        public async Task<SendSMSResponse> Get(SendSMS request)
        {
            await _smsLogic.AddAsync(request.To, request.From, request.Text);
            return new SendSMSResponse { State = StateEnum.Success };
        }

        public async Task<GetSentSMSResponse> Get(GetSentSMS request)
        {
            var filteredList = await _smsLogic.FilterByDateAsync(
                DateTime.Parse(request.DateTimeFrom),
                DateTime.Parse(request.DateTimeTo),
                request.Skip,
                request.Take);

            var sentSMSResponses = new List<SentSMSResponse>();

            foreach (var item in filteredList)
            {
                var itemCountry = (await _countryLogic.GetByMobileCodeAsync(item.MobileCountryCode));

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

        public async Task<GetStatisticsResponse> Get(GetStatistics request)
        {
            var filteredSMSList = await _smsLogic.FilterCountryDayAsync(
                DateTime.Parse(request.DateFrom),
                DateTime.Parse(request.DateTo),
                request.MccList);

            var statisticsResponses = new List<StatisticsResponse>();

            foreach (var item in filteredSMSList)
            {
                var itemCountry = await _countryLogic.GetByMobileCodeAsync(item.MobileCountryCode);

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
