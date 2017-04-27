using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mahc_Final.DBContext;

namespace Mahc_Final.DBContext
{
    public class HospitalStaffM
    {
        public Department department { get; set; }
        public List<Staff1> staff { get; set; }
    }
}