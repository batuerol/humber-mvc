using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mahc_Final.Helpers
{
    public class FileHelper
    {
        public static long GetEpochMs()
        {
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }
    }
}