using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using MittoSample.ServiceInterface;
using MittoSample.ServiceModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using MittoSample.Logic;
using MittoSample.ServiceModel.Types;
using System;

namespace MittoSample.Tests
{
    public class UnitTest
    {
        private ServiceStackHost appHost;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            appHost = new BasicAppHost().Init();
            var container = appHost.Container;

            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(":memory:", MySqlDialect.Provider));

            container.RegisterAutoWired<SMSServices>();

            container.RegisterAutoWired<CountryServices>();

            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                db.DropAndCreateTable<SMS>();
                db.DropAndCreateTable<Country>();

                var countryList = new List<Country> {
                    new Country { Name = "Germany", CountryCode = "49", MobileCountryCode = "262", PricePerSMS = 0.055m },
                    new Country { Name = "Austria", CountryCode = "43", MobileCountryCode = "232", PricePerSMS = 0.053m },new Country { Name = "Poland", CountryCode = "48", MobileCountryCode = "260", PricePerSMS = 0.032m }};

                db.InsertAll(countryList);

                var smsList = new List<SMS> {
                    new SMS{ MobileCountryCode = "262", Receiver = "+492345678945", SendDate = DateTime.Parse("2020-05-02T10:32:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "232", Receiver = "+432345678945", SendDate = DateTime.Parse("2020-05-02T10:44:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "260", Receiver = "+482345678945", SendDate = DateTime.Parse("2020-05-02T10:58:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "262", Receiver = "+492345678945", SendDate = DateTime.Parse("2020-05-02T11:12:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "232", Receiver = "+432345678945", SendDate = DateTime.Parse("2020-05-02T11:18:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "260", Receiver = "+482345678945", SendDate = DateTime.Parse("2020-05-02T11:25:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "262", Receiver = "+492345678945", SendDate = DateTime.Parse("2020-05-03T10:32:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "232", Receiver = "+432345678945", SendDate = DateTime.Parse("2020-05-03T10:44:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "260", Receiver = "+482345678945", SendDate = DateTime.Parse("2020-05-03T10:59:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "262", Receiver = "+492345678945", SendDate = DateTime.Parse("2020-05-03T11:09:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "232", Receiver = "+432345678945", SendDate = DateTime.Parse("2020-05-03T11:16:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "260", Receiver = "+482345678945", SendDate = DateTime.Parse("2020-05-03T11:32:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "262", Receiver = "+492345678945", SendDate = DateTime.Parse("2020-05-04T10:32:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "232", Receiver = "+432345678945", SendDate = DateTime.Parse("2020-05-04T10:43:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "260", Receiver = "+482345678945", SendDate = DateTime.Parse("2020-05-04T10:55:44"), Sender = "+447845694345", Text="Hi, how are you?"},
                    new SMS{ MobileCountryCode = "262", Receiver = "+492345678945", SendDate = DateTime.Parse("2020-05-04T11:21:44"), Sender = "+447845694345", Text="Hi, how are you?"}
                };

                db.InsertAll(smsList);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public async Task Can_Call_SendSMS()
        {
            var service = appHost.Container.Resolve<SMSServices>();

            var response = await service.Get(new SendSMS
            {
                From = "+446890123784",
                Text = "Hi, how are you?",
                To = "+4317421293388"
            });

            Assert.That(response.State, Is.EqualTo(StateEnum.Success));
        }

        [Test]
        public async Task Can_Call_GetCountries()
        {
            var service = appHost.Container.Resolve<CountryServices>();

            var response = await service.Get(new GetCountries());

            Assert.That(response.Result, Is.EqualTo(new List<CountryResponse>() {
                new CountryResponse{ Cc="49", Mcc="262", Name="Germany", PricePerSMS=0.055m },
                new CountryResponse{ Cc="43", Mcc="232", Name="Austria", PricePerSMS=0.053m},
                new CountryResponse{ Cc="48", Mcc="260", Name="Poland", PricePerSMS=0.032m}
            }));
        }
    }
}
