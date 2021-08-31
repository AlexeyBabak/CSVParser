using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;
using System.Text;

namespace CSVParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var records = TrackingFile.CSVParser(@"D:\Storage\TrackingFile.csv");

            //Set string limit
            int stringLimit = 10;

            foreach (TrackingFile tracking in records)
            {
                tracking.EventCity = tracking.EventCity.Truncate(stringLimit);
            }

            //Event List
            List<EventStatus> eventStatus = EventStatus.GetEventList();

            //Join Statuses
            List<TrackingFile> resultCSV = TrackingFile.GetFinalCSV(records, eventStatus);

            //Grouped by TN
            List<TrackingGrouped> resultGroupedCSV = TrackingGrouped.GetGroupedCSV(resultCSV);
            
            //Show data from final file
            foreach (var record in resultGroupedCSV)
            {
                Console.WriteLine(record.TrackNumber);
                foreach (var item in record.Events)
                {
                    Console.WriteLine($"{item.TrackingNumber}, {item.EventDate}, {item.EventStatusID}");
                }
            }

            //////Duplicate try
            //HashSet<string> ScannedRecords = new HashSet<string>();
            
            //foreach (var row in records)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    foreach (TrackingFile col in records)
            //    {
            //        sb.AppendFormat("[{0}={1}]", col, records.ToString());
            //    }
            //    if (!ScannedRecords.Add(sb.ToString()))
            //    {
            //        // This record is a duplicate.
            //    }
            //}
            // 
        }
    }
}
