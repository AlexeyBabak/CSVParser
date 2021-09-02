using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace CSVParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var records = TrackingFile.CSVParser(@"D:\Storage\TrackingFile.csv");

            //Set string limit
            const int stringLimit = 10;

            foreach (TrackingFile tracking in records)
            {
                tracking.EventCity = tracking.EventCity.Truncate(stringLimit);
            }
            //Event List
            List<EventStatus> eventStatus = EventStatus.GetEventList();

            //Join Statuses
            List<TrackingFile> resultCSVWithoutDuplicates = TrackingFile.GetFinalCSVWithoutDuplicates(TrackingFile.GetFinalCSV(records, eventStatus));

            List<TrackingGrouped> resultGroupedCSV = TrackingGrouped.GetGroupedCSV(resultCSVWithoutDuplicates);

            //Show data from final file
            foreach (var record in resultGroupedCSV)
            {
                Console.WriteLine(record.TrackNumber);
                foreach (var item in record.Events)
                {
                    Console.WriteLine($"{item.TrackingNumber}, {item.EventDate}, {item.EventStatusID}, {item.EventStatusName}, {item.EventState}, {item.EventCity}");
                }
            }

            var errorsHandler = new ErrorsHandler();


            foreach (var record in resultGroupedCSV)
            {
                var e = errorsHandler.Validate(record.Events);
            }
            
            Console.ReadLine();
        }
    }
}
