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

            //Grouped by TN
            List <TrackingGrouped> resultGroupedCSV = TrackingGrouped.GetGroupedCSV(resultCSVWithoutDuplicates);

            //Show data from final file
            foreach (var record in resultGroupedCSV)
            {
                Console.WriteLine(record.TrackNumber);
                foreach (var item in record.Events)
                {
                    Console.WriteLine($"{item.TrackingNumber}, {item.EventDate}, {item.EventStatusID}, {item.EventStatusName}, {item.EventState}, {item.EventCity}");
                }
            }

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
        }
    }

    //public class TrackingNumberComparer : IEqualityComparer<TrackingFile>
    //{
    //    public bool Equals(TrackingFile x, TrackingFile y)
    //    {
    //        if (ReferenceEquals(x, y))
    //            return true;
    //        if (ReferenceEquals(x, null))
    //            return true;
    //        if (ReferenceEquals(y, null))
    //            return true;
    //        if (x.GetType() != y.GetType())
    //            return false;

    //        return x.TrackingNumber == y.TrackingNumber && x.EventDate == y.EventDate;
    //    }

    //    public int GetHashCode(TrackingFile trackingFile)
    //    {
    //        if (ReferenceEquals(trackingFile, null))
    //            return 0;

    //        int hashTrackingNumber = trackingFile.TrackingNumber == null ? 0 : trackingFile.TrackingNumber.GetHashCode();
    //        int hashEventDate = trackingFile.EventDate == null ? 0 : trackingFile.EventDate.GetHashCode();
    //        return hashTrackingNumber ^ hashEventDate;

    //    }
    //}
}
