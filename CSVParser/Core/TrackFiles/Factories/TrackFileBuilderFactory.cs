namespace CSVParser.Core.TrackFiles.Factories
{
    public static class TrackFileBuilderFactory
    {
        public static ITrackBuilder<TrackFile> Create(TrackStatusCache cache)
        {
            return new TrackFileBuilder(cache);
        }
    }
}