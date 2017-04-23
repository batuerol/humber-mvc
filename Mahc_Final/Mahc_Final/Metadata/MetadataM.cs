using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mahc_Final.Metadata
{
    public class BlogPostMetadata
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [MinLength(5, ErrorMessage = "{0} must be at least {1} characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty!")]
        [DataType(DataType.MultilineText)]
        [MinLength(5, ErrorMessage = "{0} must be at least {1} characters")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [StringLength(50, ErrorMessage = "{0} must have length between {2}-{1}", MinimumLength = 2)]
        [Display(Name = "Excerpt")]
        public string Excerpt { get; set; }

        [Display(Name = "Preview Image")]
        [DataType(DataType.Upload)]
        public string Slug { get; set; }

        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public System.DateTime PostDate { get; set; }
    }

    public class ERParamMetadata
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name="Arrival Time")]
        public System.DateTime ArrivalTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Treatment Time")]
        public System.DateTime TreatmentTime { get; set; }
    }
}