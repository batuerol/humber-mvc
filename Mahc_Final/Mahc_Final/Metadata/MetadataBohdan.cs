using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mahc_Final.Models
{
    /* Careers feature */
    public class Job_typesMetadata
    {
        [Required]
        [Display(Name = "Job Type")]
        public string Title;
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Desc;
    }
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
    public class Job_applicationsMetadata
    {
        [Required(ErrorMessage = "Job shuold be provided")]
        [Display(Name = "Job title")]
        public int Job_id;
        [Required(ErrorMessage = "Name shuold be provided")]
        [StringLength(255, ErrorMessage = "Name must be at least 2 characters", MinimumLength = 2)]
        public string Name;
        [Required(ErrorMessage = "Email shuold be provided")]
        [EmailAddress(ErrorMessage = "Email should be valid!")]
        public string Email;
        [Required(ErrorMessage = "Phone shuold be provided")]
        [RegularExpression(@"^([\+]?[0-9]{1,3}[\s.-][0-9]{1,12})([\s.-]?[0-9]{1,4}?)$", ErrorMessage = "Phone shuold be valid")]
        public string Phone;
        [DataType(DataType.Upload)]
        public string CV;
        [DataType(DataType.MultilineText)]
        [Display(Name = "Additional information")]
        public string Text;
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> Date;
    }
    /* Volunteers feature */
    public class TaskMetadata
    {
        [Required]
        [StringLength(255, ErrorMessage = "Title must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Task title")]
        public string Title;
        [Required]
        [StringLength(255, ErrorMessage = "Type must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Task type")]
        public string Type;
        public System.DateTime Time;
        [Display(Name = "Task regularity")]
        public string Regularity;
        [Required(ErrorMessage = "Please, provide the description for current task!")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Desc;
        [Display(Name = "Published")]
        public bool Status;
        [Required]
        [StringLength(255, ErrorMessage = "Type must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Contact name")]
        public string Contact_person;
        [Required(ErrorMessage = "Contact phone shuold be provided")]
        [RegularExpression(@"^([\+]?[0-9]{1,3}[\s.-][0-9]{1,12})([\s.-]?[0-9]{1,4}?)$", ErrorMessage = "Phone shuold be valid")]
        [Display(Name = "Contact phone")]
        public string Contact_phone;
        [DataType(DataType.DateTime)]
        [Display(Name = "Created")]
        public System.DateTime Created_date;
        [Display(Name = "Created by")]
        public int Created_by;
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified")]
        public System.DateTime Modified_date;
        [Display(Name = "Modified by")]
        public int Modified_by;
    }
    public class VolunteerMetadata
    {
        [Required]
        [StringLength(255, ErrorMessage = "Name must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Your name")]
        public string Name;
        [Required]
        [StringLength(255, ErrorMessage = "Email must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Your email")]
        public string Email;
        [Required(ErrorMessage = "Contact phone shuold be provided")]
        [RegularExpression(@"^([\+]?[0-9]{1,3}[\s.-][0-9]{1,12})([\s.-]?[0-9]{1,4}?)$", ErrorMessage = "Phone shuold be valid")]
        public string Phone;
        [Display(Name = "Available time")]
        public string Pref_time;
        [Display(Name = "Prefered work")]
        public string Pref_work;
        [DataType(DataType.DateTime)]
        [Display(Name = "Application date")]
        public System.DateTime Date;
    }
    public class AlertsMetadata
    {
       
    }

    public class NewsMetadata
    {

    }
    public class EventsMetadata
    {

    }
}