using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mahc_Final.DBContext;

namespace Mahc_Final.ViewModels
{
    public class Volunteers
    {

        public List<Task> tasks { get; set; }
        public List<Volunteer> volunteers { get; set; }
    }
}