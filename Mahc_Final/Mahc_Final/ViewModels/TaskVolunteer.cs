using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mahc_Final.DBContext;

namespace Mahc_Final.ViewModels
{
    public class TaskVolunteer
    {
        public Task task { get; set; }
        public Volunteer application { get; set; }
    }
}