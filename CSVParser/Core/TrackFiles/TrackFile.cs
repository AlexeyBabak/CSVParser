using System.Collections;
using System.Collections.Generic;
using CSVParser.Core.TrackFiles.TrackBunches;

namespace CSVParser.Core.TrackFiles
{
    public class TrackFile
        : IEnumerable<TrackBunch>
    {
        public static TrackFile Empty { get; } = new();

        private readonly List<TrackBunch> _bunches = new();

        #region CTOR

        public TrackFile()
        { }

        public TrackFile(IEnumerable<TrackBunch> bunches)
        {
            _bunches.AddRange(bunches);
        }

        #endregion

        public IEnumerator<TrackBunch> GetEnumerator() => _bunches.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _bunches.GetEnumerator();
    }
}