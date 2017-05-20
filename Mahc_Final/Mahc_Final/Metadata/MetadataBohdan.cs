using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;//For editor template
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
        [UIHint("tinymce_jquery_full"),AllowHtml]//For editor template
        [Display(Name = "Description")]
        public string Desc
        {
            get;
            set;
        }
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
        [Required(ErrorMessage = "You shoould agree with privacy policy in order to process your application")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You shoould agree with privacy policy in order to process your application")]
        [Display(Name = "Agree with privacy policy")]
        public bool Agreement_;
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
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Display(Name = "Description")]
        public string Desc
        {
            get;
            set;
        }
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
        [Display(Name = "Task")]
        public int Task_id;
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
        [Required]
        [StringLength(255, ErrorMessage = "Title must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Alert title")]
        public string Title;
        [Required]
        [Display(Name = "Alert due time")]
        public System.DateTime Due_time;
        [Required]
        [DataType(DataType.MultilineText)]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Display(Name = "Description")]
        public string Desc
        {
            get;
            set;
        }
        [Display(Name = "Published")]
        public bool Status;
        [Display(Name = "Created")]
        public System.DateTime Date_created;
        [Display(Name = "Created by")]
        public int Created_by;
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified")]
        public System.DateTime Date_last_modified;
        [Display(Name = "Modified by")]
        public int Modified_by;
    }

    public class NewsMetadata
    {
        [Required]
        [StringLength(255, ErrorMessage = "Title must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Article title")]
        public string Title;
        [Required]
        [DataType(DataType.MultilineText)]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Display(Name = "Content")]
        public string Content
        {
            get;
            set;
        }
        [Display(Name = "Published")]
        public bool Published;
        [Display(Name = "Featured")]
        public bool Featured;
        [Display(Name = "Created")]
        public System.DateTime Date_created;
        [Display(Name = "Created by")]
        public int Created_by;
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified")]
        public System.DateTime Date_last_modified;
        [Display(Name = "Modified by")]
        public int Modified_by;
    }
    public class EventsMetadata
    {
        [Required]
        [StringLength(255, ErrorMessage = "Title must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Event title")]
        public string Title;
        [Required]
        [StringLength(255, ErrorMessage = "Type must be at least 2 characters", MinimumLength = 2)]
        [Display(Name = "Event type")]
        public string Type;
        [Display(Name = "Location")]
        public string Location;
        [Required]
        [Display(Name = "Event start")]
        public System.DateTime Time_start;
        [Required]
        [Display(Name = "Event end")]
        public Nullable<System.DateTime> Time_end;
        [Display(Name = "Published")]
        public bool Status;
        [Display(Name = "Featured")]
        public bool Featured;
        public string Volunteers;
        [Required]
        [DataType(DataType.MultilineText)]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Display(Name = "Description")]
        public string Desc
        {
            get;
            set;
        }
        [Display(Name = "Created")]
        public System.DateTime Date_created;
        [Display(Name = "Created by")]
        public int Created_by;
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified")]
        public System.DateTime Date_last_modified;
        [Display(Name = "Modified by")]
        public int Modified_by;
    }
}