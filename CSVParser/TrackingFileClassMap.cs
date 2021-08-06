using CsvHelper.Configuration;

namespace CSVParser
{
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
}
