using System;
using CSVParser.Core.TrackFiles.TrackBunches.TrackEvents;

namespace CSVParser.Core.TrackFiles
{
    public class ValidatorEventArgs
        : EventArgs
    {
        public static ValidatorEventArgs Successful(int rowId)
        {
            return new()
            {
                Success  = true,
                RowId    = rowId,
                Severity = SeverityLevel.Default,
                Issue    = IssueKind.None,
            };
        }

        public static ValidatorEventArgs Failure(SeverityLevel severity, IssueKind issue, string message)
        {
            return new()
            {
                Success  = false,
                Severity = severity,
                Issue    = issue,
                Message  = message 
            };
        }

        public static ValidatorEventArgs Warning(int rowId, SeverityLevel severity, IssueKind issue, string message)
        {
            return new()
            {
                Success  = false,
                RowId    = rowId, 
                Severity = severity,
                Issue    = issue,
                Message  = message 
            };
        }

        public bool Success { get; init; }
        public int RowId { get; init; }
        public SeverityLevel Severity { get; init; }
        public IssueKind Issue { get; init; }
        public string Message { get; init; }
    }

    public enum SeverityLevel
        : byte
    {
        Default,
        Warning,
        Fatal
    }

    public enum IssueKind
    {
        None,
        /// <summary>
        /// Means that <see cref="TrackEvent"/> group have issues with right event order.
        /// Some events that belong to the end of workflow appear to be in the beginning of live-time.
        /// </summary>
        WrongEventOrder,
        /// <summary>
        /// The city name is very long.
        /// </summary>
        CityNameExceedLimit,
        /// <summary>
        /// Proper TrackStatus is not found.
        /// </summary>
        TrackStatusNotExists,
        /// <summary>
        /// Some events incompatible with each other. For instance, Delivered and Returned to Client are incompatible events.
        /// </summary>
        IncompatibleEvents,
        MissingRegistrationEvent,
    }
}