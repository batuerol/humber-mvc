using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mahc_Final.Models
{
    public class JobMetadata
    {
        [Required]
        [StringLength(255, ErrorMessage = "Name must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Job Title")]
        public string Title;
        [Required]
        [Display(Name = "Job Type")]
        public int Type;
        [Required(ErrorMessage = "Please, provide the description for current job!")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Desc;
        [Display(Name = "Published")]
        public bool Status;
        [DataType(DataType.DateTime)]
        [Display(Name = "Created")]
        public Nullable<System.DateTime> Date_created;
        [Display(Name = "Created by")]
        public Nullable<int> Created_by;
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified")]
        public Nullable<System.DateTime> Date_last_modified;
        [Display(Name = "Modified by")]
        public Nullable<int> Modified_by;

    }
}