using System;
using System.Collections.Generic;
using System.Text;

namespace CSVParser
{
    public class TrackingGrouped
    {
        public string TrackNumber { get; set; }
        public List<TrackingFile> Events { get; set; }
    }
}
