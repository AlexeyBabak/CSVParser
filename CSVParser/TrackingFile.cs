using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CSVParser
{
    public class TrackingFile
    { 
        public string TrackingNumber { get; set; }
        public DateTime EventDate { get; set; }
        public int EventStatusID { get; set; }
        public string EventState { get; set; }
        public string EventCity { get; set; }
        public string EventStatusName { get; set; }

        public static List<TrackingFile> CSVParser(string path)
        {
            using var streamReader = new StreamReader(path);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Context.RegisterClassMap<TrackingFileClassMap>();
            return csvReader.GetRecords<TrackingFile>().ToList(); ;
        }

        public static List<TrackingFile> GetFinalCSV(List<TrackingFile> records, List<EventStatus> eventStatus)
        {
            return (from f in records
                    join s in eventStatus on f.EventStatusID equals s.Index into g
                    select new TrackingFile()
                    {
                        TrackingNumber = f.TrackingNumber,
                        EventDate = f.EventDate,
                        EventStatusID = f.EventStatusID,
                        EventState = f.EventState,
                        EventCity = f.EventCity,
                        EventStatusName = g.Any() ? g.First().EventStatusName : null
                    }).ToList();
        }

        public static List<TrackingFile> GetFinalCSVWithoutDuplicates(List<TrackingFile> records)
        { 
            return records
                .GroupBy(x => new { x.TrackingNumber, x.EventDate, x.EventStatusID, x.EventState, x.EventCity })
                .Select(x => x.FirstOrDefault())
                .ToList();
        }
    }
}
