using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mahc_Final.ViewModels
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Preview { get; set; }
        public string Image { get; set; }
        public string Username { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}