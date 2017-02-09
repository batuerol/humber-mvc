using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatientsMVC.Models
{
    [Table("Patients")]
    public class Patients
    {
        public int Id { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string HCnmb { get; set;}
        public DateTime DateOfB { get; set;}  

    }
}