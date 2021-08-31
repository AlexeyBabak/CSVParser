using System.Collections.Generic;

namespace CSVParser
{
    public class EventStatus
    {
        public int Index { get; set; }
        public string EventStatusName { get; set; }

        public static List<EventStatus> GetEventList()
        {
            //Create list with Status events
            return new List<EventStatus>() {
                    new EventStatus(){ Index = 1, EventStatusName = "Package Registered" },
                    new EventStatus(){ Index = 2, EventStatusName = "Package Received At Warehouse" },
                    new EventStatus(){ Index = 3, EventStatusName = "Delivered" },
                    new EventStatus(){ Index = 4, EventStatusName = "Returned To Sender" }
                };
        }
    }

}
