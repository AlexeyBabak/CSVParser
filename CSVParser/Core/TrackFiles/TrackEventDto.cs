using System;

namespace CSVParser.Core.TrackFiles
{
    public record TrackEventDto
    {
        public int RowNum { get; init; }
        public string TrackNum { get; init; }
        public DateTime EventDate { get; init; }
        public int EventStatusId { get; init; }
        public string EventState { get; init; }
        public string EventCity { get; init; }
        public string Comment { get; init; }
    }
}