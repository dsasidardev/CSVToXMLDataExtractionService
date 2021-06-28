using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CSVToXMLDataExtractionService
{
    public class CSVToXMLHelper
    {
        IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        ApplicationKeys appConfig = new ApplicationKeys();

        //this method will convert csv data to xml output file
        public void ConvertCSVToXMLData()
        {
            config.GetSection("applicationkeys").Bind(appConfig);
            var csvLocation = appConfig.CSVFileLocation;
            var xmlLocation = appConfig.XMLlocation;

            try
            {
                
                String[] FileContent = File.ReadAllLines(csvLocation);

                XElement xeData = new XElement("Root",

                from items in FileContent.Skip(1)
                let fields = items.Split(',')
                
           select new XElement("Orders",
                   new XElement("OrderNo", fields[0]),
                   new XElement("TotalValue", Convert.ToDouble(fields[10])),
                   new XElement("TotalWeight", Convert.ToDouble(fields[11])),
                   new XElement("ItemCurrency", fields[13] == null || fields[13] == "" ? "GBP" : fields[13])),
                 
                from items in FileContent.Skip(1)
                 let fields = items.Split(',')

          select   new XElement("Consignments",
                   new XElement("ConsignmentNo", fields[1]),
                    new XElement("ConsigneeName", fields[3])),

                  from items in FileContent.Skip(1)
                  let fields = items.Split(',')

          select   new XElement("Parcels",
                   new XElement("ParcelCode", fields[2])),
                   
                  from items in FileContent.Skip(1)
                    let fields = items.Split(',')

          select new XElement("ParcelItems",
                     new XElement("ItemQuantity", fields[9]),
                     new XElement("ItemDescription", fields[12]))

                );

                
                xeData.Save(Path.Combine(xmlLocation + '-' + DateTime.Now.ToString("MMddyyyy.HHmmss")));
                Console.WriteLine("File Saved to the following location:"  +xmlLocation );

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured Please Contact support with this Error:" + ex.ToString());
                Console.WriteLine("CSVToXMLDataExtractionService aborted");
            }
        }

    }
}
