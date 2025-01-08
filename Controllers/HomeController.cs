using AutoMapper;
using BlogManagement.Contracts;
using BlogManagement.Data;
using BlogManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BlogManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public HomeController( IMapper mapper, ITokenService tokenService,
            IConfiguration config, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int? page)
        {
            //get all category
            var cateList = await _unitOfWork.Categories.FindAll();
            var cateParentAndChild = cateList
                            .Where(c => c.ParentId == null)
                            .Select(c => new ShowAllChildCategoryVM()
                            {
                                Id = c.Id,
                                Title = c.Title,
                                ParentId = c.ParentId,
                                CategoryChild = cateList.Where(a => a.ParentId == c.Id).ToList()
                            })
                            .ToList();

            //pagelist
            if (page == null) page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            //get all post
            var postList = await _unitOfWork.Posts.FindAll(p => p.Published == true,includes: p =>p.Include(u => u.Users));
            var postShow = postList.Select(c => new ShowListPostVM
            {
                Id = c.Id,
                Title = c.Title,
                MetaTile = c.MetaTile,
                CreatedAt = c.CreatedAt,
                ImagePath = c.ImagePath,
                Users = c.Users

            }).ToList().OrderByDescending(c => c.Id);

            //get all hot post
            var hotPostList = await _unitOfWork.Posts.FindAll(p => p.HotPost == true);
            var hotPostListShow = hotPostList.Select(c => new ShowListPostVM
            {
                Id = c.Id,
                Title = c.Title,
                MetaTile = c.MetaTile,
                CreatedAt = c.CreatedAt,
                ImagePath = c.ImagePath,
                Users = c.Users

            }).ToList().OrderByDescending(c => c.Id);

            //show all model
            ShowCategoryPostVM model = new ShowCategoryPostVM();
            model.CategoryChilds = cateParentAndChild;
            model.ListPosts = postShow.ToPagedList(pageNumber, pageSize);
            model.ListHostPost = hotPostListShow;
            return View(model);
        }

        [Route("home/postcate")]
        public async Task<IActionResult> ShowPostCategoryById(int id,int? page, string searchString,string datecreate)
        {
            var cateList = await _unitOfWork.Categories.FindAll();
            var cateListParentAndChild = cateList
                            .Where(c => c.ParentId == null)
                            .Select(c => new ShowAllChildCategoryVM()
                            {
                                Id = c.Id,
                                Title = c.Title,
                                ParentId = c.ParentId,
                                CategoryChild = cateList.Where(a => a.ParentId == c.Id).ToList()
                            })
                            .ToList();
            //page list
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            if (page == null) page = 1;

            //search by title
            if (!String.IsNullOrEmpty(searchString) && id == 0)
            {
                var postListSearched = await _unitOfWork.Posts.FindAll(p => p.Published == true &&  p.Title.Contains(searchString), includes: p => p.Include(u => u.Users));
                var postListSerachedVM = postListSearched.Select(c => new ShowListPostVM
                {
                    Id = c.Id,
                    Title = c.Title,
                    MetaTile = c.MetaTile,
                    CreatedAt = c.CreatedAt,
                    ImagePath = c.ImagePath,
                    Users = c.Users

                }).ToList().OrderByDescending(c => c.Id);

                //get all hot post
                var hotPostList = await _unitOfWork.Posts.FindAll(p => p.HotPost == true);
                var hotPostListVM = hotPostList.Select(c => new ShowListPostVM
                {
                    Id = c.Id,
                    Title = c.Title,
                    MetaTile = c.MetaTile,
                    CreatedAt = c.CreatedAt,
                    ImagePath = c.ImagePath,
                    Users = c.Users

                }).ToList().OrderByDescending(c => c.Id);

                //show all
                ShowCategoryPostVM modelSearch = new ShowCategoryPostVM();
                modelSearch.CategoryChilds = cateListParentAndChild;
                modelSearch.ListHostPost = hotPostListVM;
                modelSearch.ListPosts = postListSerachedVM.ToPagedList(pageNumber, pageSize);
                return View(modelSearch);
            }
            else if (id !=0)
            {
                var postCateList = await _unitOfWork.PostCategories.FindAll(c => c.CategoryId == id,
                    includes: pc => pc.Include(x =>x.Posts).ThenInclude(u => u.Users));
                var postCateVM = postCateList.Select(c => c.Posts);

                var model = _mapper.Map<IEnumerable<ShowListPostVM>>(postCateVM);

                //getall hostpost
                var hotPostList = await _unitOfWork.Posts.FindAll(p => p.HotPost == true);
                var hotPostListVM = hotPostList.Select(c => new ShowListPostVM
                {
                    Id = c.Id,
                    Title = c.Title,
                    MetaTile = c.MetaTile,
                    CreatedAt = c.CreatedAt,
                    ImagePath = c.ImagePath,
                    Users = c.Users

                }).ToList().OrderByDescending(c => c.Id);

                //show all
                ShowCategoryPostVM modelCateAndPostList = new ShowCategoryPostVM();
                modelCateAndPostList.ListHostPost = hotPostListVM;
                modelCateAndPostList.CategoryChilds = cateListParentAndChild;
                modelCateAndPostList.ListPosts = model.ToPagedList(pageNumber, pageSize);
                return View(modelCateAndPostList);
            }
            else
            {
                try
                {
                    var dateCreateAt = Convert.ToDateTime(datecreate);
                    var postCateList = await _unitOfWork.Posts.FindAll(c => c.Published == true && c.CreatedAt >= dateCreateAt, includes: p => p.Include(u => u.Users));
                    var model = _mapper.Map<IEnumerable<ShowListPostVM>>(postCateList);

                    //getall hostpost
                    var hotPostList = await _unitOfWork.Posts.FindAll(p => p.HotPost == true);
                    var hotPostListVM = hotPostList.Select(c => new ShowListPostVM
                    {
                        Id = c.Id,
                        Title = c.Title,
                        MetaTile = c.MetaTile,
                        CreatedAt = c.CreatedAt,
                        ImagePath = c.ImagePath,
                        Users = c.Users

                    }).ToList().OrderByDescending(c => c.Id);

                    ShowCategoryPostVM modeListPostAndCate = new ShowCategoryPostVM();
                    modeListPostAndCate.ListHostPost = hotPostListVM;
                    modeListPostAndCate.CategoryChilds = cateListParentAndChild;
                    modeListPostAndCate.ListPosts = model.ToPagedList(pageNumber, pageSize);
                    return View(modeListPostAndCate);
                }
                catch (Exception ex)
                {
                    return (RedirectToAction("Index"));
                }
                
            }



        }
        [Authorize(Roles = "Admin")]
        [Route("home/manage")]
        public async Task<IActionResult> ManagerPostHome()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }

            var postList = await _unitOfWork.Posts.FindAll();
            var hotPostList = await _unitOfWork.Posts.FindAll(p => p.HotPost == true);
            
            var postListVM = _mapper.Map<List<Post>, List<PostVM>>(postList.ToList());
            var hotpotListVM = _mapper.Map<List<Post>, List<PostVM>>(hotPostList.ToList());

            var model = new ManageHomePost
            {
                ListAllPost = postListVM,
                ListHotPost = hotpotListVM

            };
            return View(model);
        }

        [HttpPost]
        [Route("home/manager/changestatus")]
        public async Task<JsonResult> ChangeStatus(long id)
        {
            //ajax set status post == true
            var post = await _unitOfWork.Posts.Find(p => p.Id == id);
            post.Published = !post.Published;

            _unitOfWork.Posts.Update(post);
            await _unitOfWork.Save();
           
            return Json(new
            {
                status = post.Published
            });
        }
        [HttpPost]
        [Route("home/manager/changestatushotpost")]
        public async Task<JsonResult> ChangeStatusHotPost(long id)
        {
            //ajax set hot post == true
            var post = await _unitOfWork.Posts.Find(p => p.Id == id);
            post.HotPost = !post.HotPost;

            _unitOfWork.Posts.Update(post);
            await _unitOfWork.Save();

            return Json(new
            {
                status = post.HotPost
            });
        }


    }

}
