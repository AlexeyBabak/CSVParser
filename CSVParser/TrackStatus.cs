using System;
using System.Collections.Generic;
using System.Text;

namespace CSVParser
{
    public enum TrackStatus
    {
        Undefined,
        PackageRegistered,
        PackageReceivedAtWarehouse,
        Delivered,
        ReturnedToSender
    }
}
