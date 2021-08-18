namespace CSVParser.Core.TrackFiles
{
    public enum TrackStatus
    {
        Unknown = -101,
        PackageRegistered = 1,
        PackageReceivedAtWarehouse = 2,
        Delivered = 3,
        ReturnedToSender = 4
    }
}