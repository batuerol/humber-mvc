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
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Content { get; set; }
        [Required(ErrorMessage = "You need to provide an image.")]
        [DataType(DataType.Upload)]
        public string Slug { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public System.DateTime PostDate { get; set; }
    }
}