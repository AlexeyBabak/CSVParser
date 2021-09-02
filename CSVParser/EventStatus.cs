using System.Collections.Generic;

namespace CSVParser
{
    public class EventStatus
    {
        public int Index { get; set; }
        public TrackStatus EventStatusName { get; set; }

        public static List<EventStatus> GetEventList()
        {
            //Create list with Status events
            return new List<EventStatus>() {
                    new EventStatus(){ Index = 1, EventStatusName = TrackStatus.PackageRegistered },
                    new EventStatus(){ Index = 2, EventStatusName = TrackStatus.PackageReceivedAtWarehouse },
                    new EventStatus(){ Index = 3, EventStatusName = TrackStatus.Delivered },
                    new EventStatus(){ Index = 4, EventStatusName = TrackStatus.ReturnedToSender }
                };
        }
    }

}
