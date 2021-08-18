using System;
using System.Collections.Generic;
using System.Linq;
using CSVParser.Core.TrackFiles.TrackBunches;

namespace CSVParser.Core.TrackFiles
{
    public class TrackFileBuilder 
        : ITrackBuilder<TrackFile>
    {
        private ITrackBuilder<TrackBunch> _butchBuilder;

        public event EventHandler<ValidatorEventArgs> Validated
        {
            // event "proxy"
            add => _butchBuilder.Validated += value;
            remove => _butchBuilder.Validated -= value;
        }
        public event EventHandler<ValidatorEventArgs> ValidationIssue
        {
            // event "proxy"
            add => _butchBuilder.ValidationIssue += value;
            remove => _butchBuilder.ValidationIssue -= value;
        }

        public ITrackBuilder<TrackBunch> ButchBuilder
        {
            get => _butchBuilder;
            set => _butchBuilder = value ?? throw new ArgumentNullException(nameof(value));
        }

        #region CTOR

        public TrackFileBuilder(TrackStatusCache statusCache)
        {
            _butchBuilder = new TrackBunchBuilder(statusCache);
        }

        #endregion

        public TrackFile Build(IEnumerable<TrackEventDto> from)
        {
            if (null == from) 
                throw new ArgumentNullException(nameof(from));
            var src = @from as TrackEventDto[] ?? @from.ToArray();
            if (!src.Any())
                return TrackFile.Empty;

            var butches = src
                .GroupBy(x => x.TrackNum, StringComparer.InvariantCultureIgnoreCase)
                .Select(x => ButchBuilder.Build(x));

            return new TrackFile(butches);
        }
    }
}