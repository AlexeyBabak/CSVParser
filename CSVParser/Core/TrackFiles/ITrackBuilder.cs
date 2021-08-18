using System;
using System.Collections.Generic;

namespace CSVParser.Core.TrackFiles
{
    public interface ITrackBuilder<out TResult>
    {
        event EventHandler<ValidatorEventArgs> Validated;
        event EventHandler<ValidatorEventArgs> ValidationIssue;
        TResult Build(IEnumerable<TrackEventDto> from);
    }
}