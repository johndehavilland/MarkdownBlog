using MarkdownBlog.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MarkdownBlog.Models
{

    public class BlogPostContext : DbContext
    {
        public BlogPostContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("BlogPost")]
    public class BlogPost
    {
        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BlogPostId { get; set; }

        [Required]
        [Display(Name = "Blog Title")]
        public string BlogPostTitle { get; set; }

        [Required]
        [Display(Name = "Blog Text")]
        public string BlogPostText { get; set; }

        string _html;
        [Display(Name = "Blog HTML")]
        public string BlogPostTextHTML
        {
            get
            {
                return _html;
            }
            set
            {
                IBlogConvertor convertor = new MarkDownConvertor();
                _html = convertor.Convert(BlogPostText);
            }
        }

        public string BlogPostTextHTMLTruncate
        {
            get
            {
                IBlogConvertor convertor = new MarkDownConvertor();
                return convertor.Convert(BlogPostText.Truncate(1000));
            }
        }

        [Display(Name = "Created On")]
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTime CreatedOn { get; set; }
    }
}