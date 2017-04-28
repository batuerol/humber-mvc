//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Mahc_Final.Metadata;

namespace Mahc_Final.DBContext
{
    using System;
    using System.Collections.Generic;

    [MetadataType(typeof(BlogPostMetadata))]
    public partial class BlogPost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BlogPost()
        {
            this.ParentPosts = new HashSet<BlogPost>();
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public byte[] PreviewImage { get; set; }
        public System.DateTime PostDate { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public string PostStatus { get; set; }
        public int AuthorId { get; set; }
        public Nullable<int> ParentPostId { get; set; }
        public string Slug { get; set; }

        public virtual HosMember HosMember { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogPost> ParentPosts { get; set; }
        public virtual BlogPost ParentPost { get; set; }

        public override bool Equals(object obj)
        {
            BlogPost post = (BlogPost)obj;
            return Title == post.Title && Content == post.Content && Excerpt == post.Excerpt && Slug == post.Slug;
        }
    }
}
