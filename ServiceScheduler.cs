using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Topshelf;
using System.Data;

namespace CSVToXMLDataExtractionService
{
    public class ServiceScheduler : ServiceControl
    {
        bool checkOutofHours;
        private System.Timers.Timer _timer = new System.Timers.Timer();

        //get here the appSettings json values
        IConfiguration config = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", true, true)
                  .Build();
        ApplicationKeys appConfig = new ApplicationKeys();
        CSVToXMLHelper csvToXml = new CSVToXMLHelper();

        public bool Start(HostControl hostControl)
        {
            PollTheService();
            return true;
        }

        public bool PollTheService()
        {
            config.GetSection("applicationkeys").Bind(appConfig);
            var servicePoll = appConfig.ServicePollingMinutes;
            double servMins = Convert.ToDouble(servicePoll);

            //Here we need to place 6000 factor, otherwise interval taking in millseconds and hence timer go wrong (1 sec = 1000, 30 mins = 1800000)
            TimeSpan _intervalMinutes = TimeSpan.FromMinutes(servMins) * 60000;

            Console.WriteLine("The CSVToXMLDataExtractionService Service Started!");

            if (_intervalMinutes.TotalMinutes != 0)
                _timer.Interval = (_intervalMinutes.TotalMinutes);
            _timer.Start();
            _timer.Elapsed += _timer_Elapsed;

            return true;

        }
        public bool CheckOutofHours()
        {
            bool isTrue = true;
            var serviceStart = appConfig.CSVToXMLDataExtractionServiceStart;
            var serviceEnd = appConfig.CSVToXMLDataExtractionServiceEnd;

            TimeSpan start = TimeSpan.Parse(serviceStart);
            TimeSpan end = TimeSpan.Parse(serviceEnd);
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (now >= start && now <= end)
                isTrue = false;

            return isTrue;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            checkOutofHours = CheckOutofHours();

            if (checkOutofHours)
            {
               Console.WriteLine("CSVToXMLDataExtractionService service stopped due to out of Scheduled Hours");
            }
           

            if (!checkOutofHours)
            {
                Console.WriteLine(" CSVToXMLDataExtractionService Service under progress...");
                csvToXml.ConvertCSVToXMLData();
                Console.WriteLine(" CSVToXMLDataExtractionService Service Finished its Run");

            }
        }

        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine("The CSVToXMLDataExtractionService Service Stopped!");
            _timer.Stop();
            return true;
        }



    }
}
