using AutoMapper;
using BlogManagement.Data;
using BlogManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Mappings
{
    public class Maps :Profile
    {
        public Maps()
        {
            CreateMap<Post, PostVM>().ReverseMap();
            CreateMap<Post, CreateEditPostVM>().ReverseMap();
            CreateMap<Post, ShowPostByCateVM>().ReverseMap();
            CreateMap<Post, ShowPostByIdVM>().ReverseMap(); 
            CreateMap<Post, ShowListPostVM>().ReverseMap();
            CreateMap<User, PostLoginGoogleandFacebookVM>().ReverseMap();
            CreateMap<User, RegisterVM>().ReverseMap();
            CreateMap<User, ShowUser>().ReverseMap();
            CreateMap<User, UserVM>().ReverseMap();
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Category, CategoryChildVM>().ReverseMap();
            CreateMap<Category, ShowAllChildCategoryVM>().ReverseMap();
            CreateMap<Tag, TagVM>().ReverseMap();
            CreateMap<PostComment, PostCommentVM>().ReverseMap();
            CreateMap<PostCategory, PostCategoryVM>().ReverseMap();
            CreateMap<PostCategory, ShowPostByCateVM>().ReverseMap();

        }
    }
}
