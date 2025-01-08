using BlogManagement.Data;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogManagement.Models
{
    public class PostVM
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public UserVM Users { get; set; }
        public long? ParentId { get; set; }
        public string Title { get; set; }
        public string MetaTile { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool HotPost { get; set; }

    }
    public class CreateEditPostVM
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
        public bool HotPost { get; set; }
        
        public IFormFile File { get; set; }
        public List<long> CategoriesId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

    }
    public class ShowPostByIdVM
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
        public ICollection<PostCommentVM> PostComments { get; set; }


    }
    public class ShowListPostVM
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string Title { get; set; }
        public string MetaTile { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImagePath { get; set; }
        public User Users { get; set; }

    }
    public class ManageHomePost
    {
        public ICollection<PostVM> ListHotPost{ get; set; }
        public ICollection<PostVM> ListAllPost { get; set; }


    }


}
