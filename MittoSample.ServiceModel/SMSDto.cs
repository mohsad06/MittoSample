using ServiceStack;
using System.Collections.Generic;

namespace MittoSample.ServiceModel
{
    /// <summary>
    /// SendSMS request DTO class
    /// </summary>
    [Route("/sms/send")]
    public class SendSMS : IReturn<SendSMSResponse>
    {
        /// <summary>
        /// The phone number of the SMS sender
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// The phone number of the SMS receiver
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// The text of the SMS
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// SendSMS response DTO
    /// </summary>
    public class SendSMSResponse
    {
        /// <summary>
        /// Will be false in case sending SMS fails, otherwise will be true.
        /// </summary>
        public StateEnum State { get; set; }
        /// <summary>
        /// All exceptions get injected into this property.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// GetSentSMS request DTO
    /// </summary>
    [Route("/sms/sent")]
    public class GetSentSMS : IReturn<GetSentSMSResponse>
    {
        /// <summary>
        /// The start of date range for filtering results
        /// </summary>
        public string DateTimeFrom { get; set; }
        /// <summary>
        /// The end of date range for filtering results
        /// </summary>
        public string DateTimeTo { get; set; }
        /// <summary>
        /// The number of desired records to be skipped
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// The number of desired records to be taken
        /// </summary>
        public int Take { get; set; }
    }

    /// <summary>
    /// GetSentSMS response DTO
    /// </summary>
    public class GetSentSMSResponse
    {
        /// <summary>
        /// The total number of filtered SMS records to be returned.
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// A list of all filtered SMS records.
        /// </summary>
        public List<SentSMSResponse> Items { get; set; }
        /// <summary>
        /// Will be false in case fetching SMS records fails, otherwise will be true.
        /// </summary>
        public StateEnum State { get; set; }
        /// <summary>
        /// All exceptions get injected into this property.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// Used in GetSentSMS response DTO class to generate a list of results.
    /// </summary>
    public class SentSMSResponse
    {
        /// <summary>
        /// Mapped to SendDate of the SMS class.
        /// </summary>
        public string DateTime { get; set; }
        /// <summary>
        /// Mapped to MobileCountryCode of the SMS class.
        /// </summary>
        public string Mcc { get; set; }
        /// <summary>
        /// Mapped to Sender of the SMS class.
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Mapped to Received of the SMS class.
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Mapped to PricePerSMS of the Country class, found by MobileCountryCode of SMS class.
        /// </summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// GetStatistics request DTO
    /// </summary>
    [Route("/statistics")]
    public class GetStatistics : IReturn<GetStatisticsResponse>
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string MccList { get; set; }
    }

    /// <summary>
    /// GetStatistics response DTO
    /// </summary>
    public class GetStatisticsResponse
    {
        /// <summary>
        /// The result of getting all records based on provided criteria.
        /// </summary>
        public List<StatisticsResponse> Items { get; set; }
        /// <summary>
        /// All exceptions get injected into this property.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// Used in GetStatistics response DTO class to generate a list of results.
    /// </summary>
    public class StatisticsResponse
    {
        /// <summary>
        /// The Day of the send date of sms.
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// The MobileCountryCode of the sms.
        /// </summary>
        public string Mcc { get; set; }
        /// <summary>
        /// The price of each SMS based on its MobileCountryCode and the PricePerSMS specified in Country class.
        /// </summary>
        public decimal PricePerSMS { get; set; }
        /// <summary>
        /// The number of all records returned in the specified criteria.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// The total price of returned results based on the number of sms records and price per sms specified in country.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// Used to specify the final status of reponse DTOs.
    /// </summary>
    public enum StateEnum
    {
        Failed,
        Success
    }
}
