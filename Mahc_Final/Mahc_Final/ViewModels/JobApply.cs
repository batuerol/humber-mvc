using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mahc_Final.DBContext;

namespace Mahc_Final.ViewModels
{
    public class JobApply
    {
        public Job job { get; set; }
        public Job_applications application { get; set; }
    }
}