using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mahc_Final.DBContext;

namespace Mahc_Final.ViewModels
{
    public class Careers
    {
        public List<Job> opportunities { get; set; }
        public List<Job_applications> applications { get; set; }
    }
}