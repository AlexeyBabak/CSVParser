using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSVParser
{
    public class TrackingGrouped
    {
        public string TrackNumber { get; set; }
        public List<TrackingFile> Events { get; set; }

        public static List<TrackingGrouped> GetGroupedCSV(List<TrackingFile> resultCSV)
        {
            return resultCSV
                .GroupBy(u => u.TrackingNumber)
                .Select(grp => new TrackingGrouped() { TrackNumber = grp.Key, Events = grp.ToList() })
                .ToList();
        }
    }
}
