using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSVParser.Core.TrackFiles.Factories;
using CSVParser.Services;

namespace CSVParser.UseCases
{
    public interface IProcessFileUseCase
    {
        Task HandleTrackFileAsync(Stream input, CancellationToken cancellationToken = default);
    }

    class ProcessFileUseCase
        : IProcessFileUseCase
    {
        private readonly ITrackFileImportService _importService;
        private readonly ITrackFileExportService _exportService;

        public async Task HandleTrackFileAsync(Stream input, CancellationToken cancellationToken = default)
        {
            var dto = await _importService.ImportAsync(input, cancellationToken);

            var trackFileBuilder = TrackFileBuilderFactory.Create(null);
            // TODO: Subscribe on validation events and show them on the screen if needed
            var trackFile = trackFileBuilder.Build(from: dto);

            await _exportService.ExportAsync(trackFile, cancellationToken);
        }
    }
}