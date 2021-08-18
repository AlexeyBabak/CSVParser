using System;
using AutoFixture;
using CSVParser.Core.TrackFiles.TrackBunches.TrackEvents;

namespace CSVParser.Core.TrackFiles.Fixtures
{
    public class TrackEventFixture
    {
        public static TrackEvent Create(Action<TrackEventFixture> mutator)
        {
            var builder = new TrackEventFixture();
            mutator?.Invoke(builder);
            return builder.Create();
        }

        private static readonly Fixture Fixture = new();

        private string _tn = Fixture.Create<string>();
        private DateTime _ed = Fixture.Create<DateTime>();
        private TrackStatus _st = TrackStatus.Unknown;
        private TrackAddress _ta = Fixture.Create<TrackAddress>();
        private string _comment = Fixture.Create<string>();

        public TrackEvent Create()
        {
            return new ()
            {
                TrackNum  = _tn,
                EventDate = _ed,
                Status    = _st,
                Address   = _ta,
                Comment   = _comment
            };
        }

        public TrackEventFixture EventDate(DateTime eventDate)
        {
            _ed = eventDate;
            return this;
        }
        public TrackEventFixture AddToEventDate(int sec = 1)
        {
            _ed = _ed.AddSeconds(sec);
            return this;
        }
        public TrackEventFixture Status(TrackStatus status)
        {
            _st = status;
            return this;
        }
    }
}