using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncher
{
    public enum SyncResolution
    {
        dontOverwrite,
        overwrite
    }

    public enum SyncDirection
    {
        OneWay,
        TwoWay
    }

    public enum SyncOperation
    {
        Move,
        Copy,
        LogOnly
    }
}
