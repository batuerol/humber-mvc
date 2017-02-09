using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PatientsMVC.Models
{
    public class PatientsContext: DbContext
    {
        public DbSet<Patients> Patients { get; set; }
    }
}