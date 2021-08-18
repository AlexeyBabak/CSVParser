using System;
using System.Collections.Generic;

namespace CSVParser.Core.TrackFiles.TrackBunches.TrackEvents
{
    public class TrackEvent
    {
        #region Equality Comparers

        private sealed class TrackNumEventDateStatusEqualityComparer 
            : IEqualityComparer<TrackEvent>
        {
            public bool Equals(TrackEvent x, TrackEvent y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.TrackNum, y.TrackNum, StringComparison.InvariantCultureIgnoreCase)
                    && x.EventDate.Equals(y.EventDate) 
                    && Equals(x.Status, y.Status);
            }

            public int GetHashCode(TrackEvent obj)
            {
                return HashCode.Combine(obj.TrackNum, obj.EventDate, obj.Status);
            }
        }

        public static IEqualityComparer<TrackEvent> TrackNumEventDateStatusComparer { get; } = new TrackNumEventDateStatusEqualityComparer();

        #endregion

        public string TrackNum { get; set; }
        public DateTime EventDate { get; set; }
        public TrackStatus Status { get; set; }
        public TrackAddress Address { get; set; }
        public string Comment { get; set; }

        public IEnumerable<ValidatorEventArgs> Validate()
        {
            if (50 < Address.City.Length)
            {
                yield return new ValidatorEventArgs
                    {Severity = SeverityLevel.Warning, Issue = IssueKind.CityNameExceedLimit};
            }

            if (TrackStatus.Unknown == Status)
            {
                yield return new ValidatorEventArgs
                    {Severity = SeverityLevel.Warning, Issue = IssueKind.TrackStatusNotExists};
            }
        }
    }
}