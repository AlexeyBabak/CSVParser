using System;
using System.Collections.Generic;
using System.Linq;
using CSVParser.Core.TrackFiles.TrackBunches.Automatonymous;
using CSVParser.Core.TrackFiles.TrackBunches.TrackEvents;

namespace CSVParser.Core.TrackFiles.TrackBunches
{
    public class TrackBunch
    {
        private DateTime? _registered;
        private DateTime? _receivedAtWh;
        private DateTime? _delivered;
        private DateTime? _returned;
        private readonly List<ValidatorEventArgs> _errors = new();

        public List<TrackEvent> Events { get; } = new();

        public TrackBunchState Registered { get; private set; }
        public TrackBunchState ReceivedAtWh { get; private set; }

        public TrackBunch(IEnumerable<TrackEvent> events)
        {
            Events.AddRange(events);
        }

        public IEnumerable<ValidatorEventArgs> Validate()
        {
            // 1. Registration
            Register(GetFirstEvent(TrackStatus.PackageRegistered));

            // 2. Received at Warehouse
            ReceivedAtWarehouse(GetFirstEvent(TrackStatus.PackageReceivedAtWarehouse));

            // 3. Delivered
            Delivered(GetFirstEvent(TrackStatus.Delivered));

            // 4. Returned to sender
            Returned(GetFirstEvent(TrackStatus.ReturnedToSender));

            return _errors;
        }

        private void Register(TrackEvent trackEvent)
        {
            if (null != trackEvent)
            {
                _registered = trackEvent.EventDate;
            }
            else
            {
                AddError(IssueKind.MissingRegistrationEvent);
            }
        }

        private void ReceivedAtWarehouse(TrackEvent trackEvent)
        {
            if (_registered.HasValue && null != trackEvent)
            {
                _receivedAtWh = trackEvent.EventDate;

                if (!(_registered <= _receivedAtWh))
                    AddError(IssueKind.WrongEventOrder,
                        $"{TrackStatus.PackageReceivedAtWarehouse:G} date stamp is '{_receivedAtWh}' but {TrackStatus.PackageRegistered:G} date stamp is '{_registered}'");
            }
            // do nothing
        }

        private void Delivered(TrackEvent trackEvent)
        {
            if (null != trackEvent)
            {
                _delivered = trackEvent.EventDate;

                if (!(null != _registered && _registered <= _delivered))
                {
                    AddError(IssueKind.WrongEventOrder,
                        $"{TrackStatus.Delivered:G} date stamp is '{_delivered}' but {TrackStatus.PackageRegistered:G} date stamp is '{_registered}'");
                }

                if (!(_receivedAtWh.HasValue && _receivedAtWh <= _delivered))
                {
                    AddError(IssueKind.WrongEventOrder,
                        $"{TrackStatus.Delivered:G} date stamp is '{_delivered}' but {TrackStatus.PackageReceivedAtWarehouse:G} date stamp is '{_receivedAtWh}'");
                }
            }
        }

        private void Returned(TrackEvent trackEvent)
        {
            if (null != trackEvent)
            {
                _returned = trackEvent.EventDate;

                if (!(null != _registered && _registered <= _returned))
                {
                    AddError(IssueKind.WrongEventOrder,
                        $"{TrackStatus.ReturnedToSender:G} date stamp is '{_returned}' but {TrackStatus.PackageRegistered:G} date stamp is '{_registered}'");
                }
                // no other statuses need validation

                if (_delivered.HasValue)
                {
                    AddError(IssueKind.IncompatibleEvents,
                        $"A box can not have both {TrackStatus.Delivered:G} and {TrackStatus.ReturnedToSender:G} statuses");
                }
            }
        }

        private TrackEvent GetFirstEvent(TrackStatus status)
        {
            return Events.FirstOrDefault(x => x.Status == status);
        }

        private void AddError(IssueKind issueKind, string comment = null)
        {
            var e = ValidatorEventArgs.Failure(SeverityLevel.Fatal, issueKind, comment);
            _errors.Add(e);
        }
    }

    public class TrackBunchState
        : StateMachineState
    {
        public State Registered { get; private set; }
    }
}
