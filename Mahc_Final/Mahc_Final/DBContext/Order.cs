//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mahc_Final.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Order
    {
        public int Id { get; set; }
        [Display(Name = "Your First Name")]
        [Required]
        public string sender_first_name { get; set; }
        [Display(Name = "Your Last Name")]
        [Required]
        public string sender_last_name { get; set; }
        [Display(Name = "Your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        [StringLength(10)]
        public string sender_phone { get; set; }
        [Display(Name = "Email")]
        [Required]
        [StringLength(10)]
        public string sender_email { get; set; }

        [Display(Name = "Patient's First Name")]
        [Required]

        public string patient_first_name { get; set; }
        [Display(Name = "Patient's Last Name")]
        [Required]
        public string patient_last_name { get; set; }
        [Display(Name = "Your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        [StringLength(10)]
        public string patient_phn_num { get; set; }
        [Display(Name = "Selected Gift Id")]
        public int gift_id { get; set; }
        [Display(Name = "Say Something")]
        public string message { get; set; }
    
        public virtual Gift Gift { get; set; }
    }
}
