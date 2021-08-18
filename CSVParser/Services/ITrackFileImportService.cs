using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSVParser.Core;
using CSVParser.Core.TrackFiles;

namespace CSVParser.Services
{
    public interface ITrackFileImportService
    {
        IEnumerable<TrackEventDto> Import(Stream file);
        Task<IEnumerable<TrackEventDto>> ImportAsync(Stream file, CancellationToken cancellationToken = default);
    }
}