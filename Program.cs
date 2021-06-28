using System;
using Topshelf;

namespace CSVToXMLDataExtractionService
{
    class Program
    {
       
        static void Main(string[] args)
        {
           
            HostFactory.Run(x =>
            {
                x.Service<ServiceScheduler>();
                x.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(10)));
                x.SetServiceName("CSVToXMLDataExtractionService");
                x.SetDisplayName("CSVToXMLDataExtractionService");
                x.SetDescription("This service Runs every minute/hourly and with designated timeframes & configurable through appsettings.");
                x.StartAutomatically();
            }
    );
        }
    }
}
