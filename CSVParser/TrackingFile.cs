using System;

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
    }
}
