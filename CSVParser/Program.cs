using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;
using System.Text;

namespace CSVParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var streamReader = new StreamReader(@"D:\Storage\TrackingFile.csv");
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Context.RegisterClassMap<TrackingFileClassMap>();
                var records = csvReader.GetRecords<TrackingFile>().ToList();

                //Set string limit
                int stringLimit = 10;

                foreach (TrackingFile tracking in records)
                {
                    tracking.EventCity = tracking.EventCity.Truncate(stringLimit);
                }

                ////Duplicate try
                //HashSet<string> ScannedRecords = new HashSet<string>();
                //foreach (TrackingFile tracking in records)
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

                //TODO: Add logic for date conversion

                //Show all data
                foreach (TrackingFile tracking in records)
                {
                    Console.WriteLine($"TrackingNumber: {tracking.TrackingNumber}, EventDate: {tracking.EventDate}, EventStatusID: {tracking.EventStatusID}, EventState: {tracking.EventState}, EventCity: {tracking.EventCity}");
                }

                //Create list with Status events
                List<EventStatus> eventStatus = new List<EventStatus>() {
                    new EventStatus(){ Index = 1, EventStatusName = "Package Registered" },
                    new EventStatus(){ Index = 2, EventStatusName = "Package Received At Warehouse" },
                    new EventStatus(){ Index = 3, EventStatusName = "Delivered" },
                    new EventStatus(){ Index = 4, EventStatusName = "Returned To Sender" }
                };

                //Show data from Status events
                foreach (EventStatus status in eventStatus)
                {
                    Console.WriteLine($"Index: {status.Index}, EventStatusName: {status.EventStatusName}");
                }
                
                //Join Statuses
                var resultCSV = from f in records
                                join s in eventStatus on f.EventStatusID equals s.Index into g
                                select new
                                {
                                    f.TrackingNumber,
                                    f.EventDate,
                                    f.EventStatusID,
                                    f.EventState,
                                    f.EventCity,
                                    EventStatusName = g.Any() ? g.First().EventStatusName : null
                                };

                //Show data from final file
                foreach (var record in resultCSV)
                {
                    Console.WriteLine($"TrackingNumber: {record.TrackingNumber}, EventDate: {record.EventDate}, EventStatusID: {record.EventStatusID}, EventState: {record.EventState}, EventCity: {record.EventCity}, EventStatusName: {record.EventStatusName}");
                }
            }
        }
    }

    public class TrackingFileClassMap : ClassMap<TrackingFile>
    {
        public TrackingFileClassMap()
        {
            Map(m => m.TrackingNumber).Name("TrackingNumber");
            Map(m => m.EventDate).Name("EventDate");
            Map(m => m.EventStatusID).Name("EventStatusId");
            Map(m => m.EventState).Name("EventState");
            Map(m => m.EventCity).Name("EventCity");
        }
    }

    public class TrackingFile
    { 
        public string TrackingNumber { get; set; }
        public string EventDate { get; set; }
        public int EventStatusID { get; set; }
        public string EventState { get; set; }
        public string EventCity { get; set; }
        public string EventStatusName { get; set; }
    }

    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    public class EventStatus
    {
        public int Index { get; set; }
        public string EventStatusName { get; set; }
    }
}
