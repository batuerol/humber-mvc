using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mahc_Final.DBContext;

namespace Mahc_Final.Models
{
    public class UserInfo
    {

        public string CardNumber { get; set; }
        public string ExpYear { get; set; }
        public string ExpMonth { get; set; }
        public string Cvc { get; set; }
    }
}