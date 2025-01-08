using AutoMapper;
using BlogManagement.Contracts;
using BlogManagement.Data;
using BlogManagement.Models;
using BlogManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;


        public PostController(IMapper mapper, ITokenService tokenService,
            IConfiguration config, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _config = config;
            _unitOfWork = unitOfWork;
        }


        // GET: PostControllers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }
            var postList = await _unitOfWork.Posts.FindAll();

            var model = _mapper.Map<List<Post>, List<PostVM>>(postList.ToList());
            return View(model);
        }

        [Authorize(Roles = "Admin,Blogger")]
        [Route("user/manager/post/blogger")]
        public async Task<IActionResult> ShowPostByIdBlogger()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }

            var userId = int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);
            var postList = await _unitOfWork.Posts.FindAll(p => p.Users.Id == userId, null, includes: p => p.Include(u => u.Users));

            var model = _mapper.Map<List<Post>, List<PostVM>>(postList.ToList());
            return View(model);
        }

        // GET: PostController/Details/5

        [Authorize(Roles = "Admin,Blogger")]
        [Route("user/manager/post/details")]
        public async Task<IActionResult> Details(long id)
        {
            var isExistsId = await _unitOfWork.Posts.IsExists(p => p.Id == id);
            if (!isExistsId)
            {
                return NotFound();
            }

            var post = await _unitOfWork.Posts.Find(p => p.Id == id);
            var postComment = post.PostComments;
            var model = _mapper.Map<PostVM>(postComment);

            return View(model);
        }


        // GET: PostController/Create
        [Authorize(Roles = "Admin,Blogger")]
        [Route("user/manager/post/create")]
        public async Task<IActionResult> Create()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }

            var cateChild = await _unitOfWork.Categories.FindAll();
            var cateparentItem = cateChild.Select(q => new SelectListItem
            {
                Text = q.Title,
                Value = q.Id.ToString(),
            });

            var modelCate = new CreateEditPostVM
            {
                Categories = cateparentItem,

            };

            return View(modelCate);
        }

        //POST: PostController/Create
        [Authorize(Roles = "Admin,Blogger")]
        [Route("user/manager/post/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditPostVM model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);

                }

                if (model.File == null)
                {
                    ModelState.AddModelError("", "Please choose a image ...");
                    return View(model);
                }

                if (model.CategoriesId == null)
                {
                    ModelState.AddModelError("", "Please choose a Category ...");
                    return View(model);
                }

                //create instance updateimage
                var uploadImage = new UploadImage();
                uploadImage.UpLoandImageFromFile(model);

                //get userid to post 
                var userId = int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);

                if (userId == null)
                {
                    ModelState.AddModelError("", "Please login to create a post...");
                    return View(model);
                }

                var post = _mapper.Map<Post>(model);
                post.CreatedAt = DateTime.Now;
                post.AuthorId = userId;
                post.HotPost = false;
                //check create is success 

                await _unitOfWork.Posts.Create(post);
                await _unitOfWork.Save();

                var postcate = new PostCategoryVM();
                postcate.PostId = post.Id;
                foreach (var item in model.CategoriesId)
                {
                    postcate.CategoryId = item;
                    var postCategoryList = _mapper.Map<PostCategory>(postcate);


                    await _unitOfWork.PostCategories.Create(postCategoryList);
                    await _unitOfWork.Save();
                }

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        [AllowAnonymous]
        [Route("home/post")]
        public async Task<IActionResult> ShowPostById(long id)
        {
            var existsId = await _unitOfWork.Posts.IsExists(p => p.Id == id);
            if (!existsId)
            {
                return NotFound();
            }
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

            var postList = await _unitOfWork.Posts.Find(p => p.Id == id);

            var model = _mapper.Map<ShowPostByIdVM>(postList);

            var postCommentParent = await _unitOfWork.PostComments.FindAll(pcm => pcm.PostId == postList.Id,
                orderBy: pcm => pcm.OrderByDescending(pcm => pcm.Id),
                includes: pcm => pcm.Include(p => p.Posts).Include(u => u.Users));
            var avergePoint = postCommentParent.Select(c => c.TotalVotes).Average();
            var totalUserVote = postCommentParent.Where(c => c.TotalVotes != null).Select(c => c.UserId).Count();

            var postCommentParentChild = postCommentParent
               .Where(c => c.ParentId == null)
               .Select(c => new PostCommentVM()
               {
                   Id = c.Id,
                   Title = c.Title,
                   ParentId = c.ParentId,
                   Content = c.Content,
                   CreatedAt = c.CreatedAt,
                   TotalVotes = c.TotalVotes,
                   TimeCmt = ((DateTime.Now - c.CreatedAt).TotalDays).ToString("#"),
                   Users = c.Users,
                   PostCommentsChild = postCommentParent.Where(a => a.ParentId == c.Id).ToList()
               })
               .ToList();

            model.PostComments = postCommentParentChild;
            var postCateListVM = new ShowCategoryPostVM
            {
                Id = model.Id,
                AuthorId = model.AuthorId,
                Title = model.Title,
                MetaTile = model.MetaTile,
                Slug = model.Slug,
                Summary = model.Summary,
                Published = model.Published,
                CreatedAt = model.CreatedAt,
                Content = model.Content,
                ImagePath = model.ImagePath,
                AvgVote = avergePoint,
                PostComments = model.PostComments,
                CategoryChilds = cateParentAndChild,
            };

            return View(postCateListVM);
        }

        // GET: PostController/Edit/5
        [Authorize(Roles = "Admin,Blogger")]
        [Route("user/manager/post/edit")]
        public async Task<IActionResult> Edit(long id)
        {
            var existsId = await _unitOfWork.Posts.IsExists(p => p.Id == id);
            if (!existsId)
            {
                return NotFound();
            }

            var post = await _unitOfWork.Posts.Find(p => p.Id == id);
            var model = _mapper.Map<CreateEditPostVM>(post);

            var cateChild = await _unitOfWork.Categories.FindAll(c => c.Parents == null);

            var categoryList = await _unitOfWork.PostCategories.FindAll(p => p.PostId == id);
            var category = categoryList.Select(c => c.CategoryId).ToList();
            var cateParentItem = cateChild.Select(q => new SelectListItem
            {
                Text = q.Title,
                Value = q.Id.ToString(),
            });
            model.CategoriesId = category;
            model.Categories = cateParentItem;
            return View(model);

        }

        // POST: PostController/Edit/5
        [Authorize(Roles = "Admin,Blogger")]
        [Route("user/manager/post/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditPostVM model)
        {
            try
            {
                //If there is a change in the image
                //then create a new instance to upload the image
                if (!ModelState.IsValid)
                {
                    return View(model);

                }
                if (model.File != null)
                {
                    //create instance updateimage
                    var uploadImage = new UploadImage();
                    uploadImage.UpLoandImageFromFile(model);

                }
                if (model.CategoriesId == null)
                {
                    ModelState.AddModelError("", "Please choose a Category ...");
                    return View(model);
                }

                var post = _mapper.Map<Post>(model);
                _unitOfWork.Posts.Update(post);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: PostController/Delete/5
        [Authorize(Roles = "Admin,Blogger")]
        [Route("user/manager/post/delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var post = await _unitOfWork.Posts.Find(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            _unitOfWork.Posts.Delete(post);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // POST: PostController/Delete/5
        [Authorize(Roles = "Admin,Blogger")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, PostVM model)
        {
            try
            {
                var post = await _unitOfWork.Posts.Find(p => p.Id == id);
                if (post == null)
                {
                    return NotFound();
                }

                _unitOfWork.Posts.Delete(post);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
