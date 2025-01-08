using BlogManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BlogManagement.Models
{
    public class PostCategoryVM
    {
        public long PostId { get; set; }
        public PostVM Posts { get; set; }
        public long CategoryId { get; set; }
        public CategoryVM Categories { get; set; }
    }
    public class ShowCategoryPostVM
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public long? ParentId { get; set; }
        public string Title { get; set; }
        public string MetaTile { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public double? AvgVote { get; set; }
        public ICollection<PostCommentVM> PostComments { get; set; }
        public IEnumerable<ShowAllChildCategoryVM> CategoryChilds { get; set; }
        public IPagedList<ShowListPostVM> ListPosts { get; set; }
        public IEnumerable<ShowListPostVM>? ListHostPost { get; set; }

    }
    public class ShowPostByCateVM
    {
        public IEnumerable<PostVM> ListPosts { get; set; }

    }
}
