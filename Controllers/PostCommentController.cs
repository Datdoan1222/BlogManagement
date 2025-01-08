using AutoMapper;
using BlogManagement.Contracts;
using BlogManagement.Data;
using BlogManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    public class PostCommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;


        public PostCommentController(IMapper mapper, ITokenService tokenService,
            IConfiguration config, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _config = config;
            _unitOfWork = unitOfWork;
        }
        // GET: PostCommentController

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }

            var postCommentList = await _unitOfWork.PostComments.FindAll();
            var model = _mapper.Map<List<PostComment>, List<PostCommentVM>>(postCommentList.ToList());

            return View(model);
        }

        // GET: PostCommentController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: PostCommentController/Create
        public async Task<IActionResult> Create()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index", "Home"));
            }

            var userId = int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);

            var postId = Convert.ToInt64(Request.Form["Id"]);
            var idCommentParent = Convert.ToInt64(Request.Form["IdCmtParent"]);
            var name = Request.Form["Name"];

            var checkUserVoted = await _unitOfWork.PostComments.IsExists(q => q.TotalVotes != null && 
            q.PostId == postId && q.UserId == userId);

            var valueVote = Convert.ToInt32(Request.Form["inlineFormCustomSelect"]);
            if (valueVote != 0)

            {
                if (checkUserVoted)
                {
                    ModelState.AddModelError("", "Please login to create a post...");
                    return RedirectToAction("ShowPostById", "Post", new { @id = postId });
                }
            }

            return await CreatePostComment(postId, name, idCommentParent, valueVote);
        }

        // POST: PostCommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePostComment(long postId, string name, long? idCommentParent, int? valueVote)
        {
            try
            {
                var userId = int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);

                if (userId == null)
                {
                    ModelState.AddModelError("", "Please login to create a post...");
                    RedirectToAction("Index", "Home");
                }
                PostComment postcomment = new PostComment();

                if (idCommentParent != 0)
                {
                    postcomment.ParentId = idCommentParent;
                }
                if (valueVote != 0)
                {
                    postcomment.TotalVotes = valueVote;
                }

                postcomment.CreatedAt = DateTime.Now;
                postcomment.UserId = userId;
                postcomment.Title = "Comment";
                postcomment.PostId = postId;
                postcomment.Content = name;
                postcomment.Published = true;

                // create 
                await _unitOfWork.PostComments.Create(postcomment);
                await _unitOfWork.Save();

                return RedirectToAction("ShowPostById", "Post", new { @id = postId });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View();


            }
        }

        // GET: PostCommentController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostCommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostCommentController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _unitOfWork.PostComments.Find(pc => pc.Id == id);
            if (post == null)
            {
                return NotFound();
            }

             _unitOfWork.PostComments.Delete(post);
            await _unitOfWork.Save();
            
            return RedirectToAction(nameof(Index));
        }


        // POST: PostCommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, PostCommentVM entity)
        {
            try
            {
                var post = await _unitOfWork.PostComments.Find(pc => pc.Id == id);
                if (post == null)
                {
                    return NotFound();
                }

                _unitOfWork.PostComments.Delete(post);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }
    }
}
