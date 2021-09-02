using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSVParser
{
    public class ErrorsHandler
    {
        private readonly List<EventValidator> _errors = new();

        private DateTime? _registered;
        private DateTime? _receivedAtWh;
        private DateTime? _delivered;
        private DateTime? _returned;

        public IEnumerable<EventValidator> Validate(List<TrackingFile> tracking)
        {
            var firstRegisteredEvent = GetFirstEvent(TrackStatus.PackageRegistered, tracking);
            Registration(firstRegisteredEvent);

            var firstRecievedAtWaregouseEvent = GetFirstEvent(TrackStatus.PackageReceivedAtWarehouse, tracking);
            ReceivedAtWarehouse(firstRecievedAtWaregouseEvent);

            var firstDeliveredEvent = GetFirstEvent(TrackStatus.Delivered, tracking);
            Delivered(firstDeliveredEvent);

            var firstReturnedEvent = GetFirstEvent(TrackStatus.ReturnedToSender, tracking);
            Returned(firstReturnedEvent);

            var firstUndefinedEvent = GetFirstEvent(TrackStatus.Undefined, tracking);
            Undefined(firstUndefinedEvent);

            return _errors;
        }

        public void Registration(TrackingFile trackingGrouped) 
        {
            if (trackingGrouped == null)
            {
                AddError(EventValidator.IssueType.MissingRegistration);
            }
            else 
            {
                _registered = trackingGrouped.EventDate;
            }
        }

        public void ReceivedAtWarehouse(TrackingFile trackingGrouped)
        {
            if (trackingGrouped != null)
            {
                _receivedAtWh = trackingGrouped.EventDate;

                if (_receivedAtWh <= _registered)
                {
                    AddError(EventValidator.IssueType.WrongEventOrder, $"{TrackStatus.PackageRegistered} date: {_registered} after {TrackStatus.PackageReceivedAtWarehouse} date: {_receivedAtWh}");
                }
            }
        }

        public void Delivered(TrackingFile trackingGrouped)
        {
            if (trackingGrouped != null)
            {
                _delivered = trackingGrouped.EventDate;

                //TODO: add logic
            }
        }

        public void Returned(TrackingFile trackingGrouped)
        {
            if (trackingGrouped != null)
            {
                _returned = trackingGrouped.EventDate;

                //TODO: add logic
            }
        }

        public void Undefined(TrackingFile trackingGrouped)
        {
            if (trackingGrouped != null)
            {
                AddError(EventValidator.IssueType.EventMissing, $"Undefined Event for Tracking Number: {trackingGrouped.TrackingNumber}. ID: {trackingGrouped.EventStatusID}");
            }
        }

        public void AddError(EventValidator.IssueType issueKind, string comment = null)
        {
            var error = EventValidator.Failure(issueKind, comment);
             _errors.Add(error);
        }

        public TrackingFile GetFirstEvent(TrackStatus status, List<TrackingFile> events)
        {
            return events.FirstOrDefault(x => x.EventStatusName == status);
        }
    }
}
