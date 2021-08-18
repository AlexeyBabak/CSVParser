using System.Threading;
using System.Threading.Tasks;
using CSVParser.Core.TrackFiles;

namespace CSVParser.Services
{
    public interface ITrackFileExportService
    {
        void Export(TrackFile file);
        Task ExportAsync(TrackFile file, CancellationToken cancellationToken = default);
    }
}