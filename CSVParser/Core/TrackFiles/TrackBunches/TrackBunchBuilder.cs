using System;
using System.Collections.Generic;
using System.Linq;
using CSVParser.Core.TrackFiles.TrackBunches.TrackEvents;

namespace CSVParser.Core.TrackFiles.TrackBunches
{
    public class TrackBunchBuilder
        : ITrackBuilder<TrackBunch>
    {
        private readonly TrackStatusCache _statusCache;

        public event EventHandler<ValidatorEventArgs> Validated;
        public event EventHandler<ValidatorEventArgs> ValidationIssue;

        public TrackBunchBuilder(TrackStatusCache statusCache)
        {
            _statusCache = statusCache
                ?? throw new ArgumentNullException(nameof(statusCache));
        }

        public TrackBunch Build(IEnumerable<TrackEventDto> @from)
        {
            var trackEvents = Map(@from);
            // TODO: should some event be raised for duplicates?
            var bunch = new TrackBunch(trackEvents.Distinct(TrackEvent.TrackNumEventDateStatusComparer));
            Validate(bunch);
            return bunch;
        }

        private IEnumerable<TrackEvent> Map(IEnumerable<TrackEventDto> @from) => @from?.Select(Map) ?? throw new ArgumentNullException(nameof(@from));

        private TrackEvent Map(TrackEventDto @from)
        {
            var trackEvent = new TrackEvent
            {
                TrackNum  = @from.TrackNum,
                EventDate = @from.EventDate,
                Status    = Map(@from.EventStatusId),
                Address   = Map(@from.EventState, @from.EventCity),
                Comment   = @from.Comment
            };

            OnValidated(ValidatorEventArgs.Successful(@from.RowNum));

            foreach (var warning in trackEvent.Validate())
            {
                var e = ValidatorEventArgs.Warning(@from.RowNum, warning.Severity, warning.Issue, warning.Message);
                OnValidationIssue(e);
            }

            return trackEvent;
        }

        private TrackStatus Map(int statusId)
        {
            return _statusCache.TryGetValue(statusId, out TrackStatus res) 
                ? res 
                : TrackStatus.Unknown;
        }
        
        private TrackAddress Map(string state, string city)
        {
            return new(state, city);
        }

        private void Validate(TrackBunch bunch)
        {
            var issues = bunch.Validate();
            foreach (var issue in issues)
            {
                OnValidationIssue(issue);
            }
        }

        protected virtual void OnValidated(ValidatorEventArgs e)
        {
            Validated?.Invoke(this, e);
        }

        protected virtual void OnValidationIssue(ValidatorEventArgs e)
        {
            ValidationIssue?.Invoke(this, e);
        }
    }
}