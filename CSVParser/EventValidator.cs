using System;
using System.Collections.Generic;
using System.Text;

namespace CSVParser
{
    public class EventValidator
    {
        public static EventValidator Failure(IssueType issue, string message)
        {
            return new()
            {
                Issue = issue,
                Message = message
            };
        }

        public IssueType Issue { get; set; }
        public string Message { get; set; }

        public enum IssueType
        {
            None,
            WrongEventOrder,
            MissingRegistration,
            TrackStatusNotExists,
            IncompatibleEvents,
            EventMissing
        }
    }
}
