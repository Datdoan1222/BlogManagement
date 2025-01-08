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
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("user/manager/tag")]

    public class TagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public TagController(IMapper mapper, ITokenService tokenService,
            IConfiguration config,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }
            var tagList = await _unitOfWork.Tags.FindAll();

            var model = _mapper.Map<List<Tag>, List<TagVM>>(tagList.ToList());
            return View(model);
        }

        [Route("create")]

        public IActionResult Create()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }
            return View();
        }

        // POST: CategoryController/Create
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagVM model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);

                }
                //get userid to post 

                var tag = _mapper.Map<Tag>(model);

                //check create is success 
                await _unitOfWork.Tags.Create(tag);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: CategoryController/Edit/5
        [Route("edit")]

        public async Task<IActionResult> Edit(int id)
        {
            var existsId = await _unitOfWork.Tags.IsExists(t => t.Id == id);
            if (!existsId)
            {
                return NotFound();
            }

            var tag = await _unitOfWork.Tags.Find(t => t.Id == id);
            var model = _mapper.Map<TagVM>(tag);
            return View(model);
        }

        // POST: CategoryController/Edit/5
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagVM model)
        {
            try
            {
                //If there is a change in the image
                //then create a new instance to upload the image
                if (!ModelState.IsValid)
                {
                    return View(model);

                }

                var tag = _mapper.Map<Tag>(model);
                _unitOfWork.Tags.Update(tag);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: CategoryController/Delete/5
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _unitOfWork.Tags.Find(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            _unitOfWork.Tags.Delete(tag);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // POST: CategoryController/Delete/5
        [Route("delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, TagVM entity)
        {
            try
            {
                var tag = await _unitOfWork.Tags.Find(t => t.Id == id);
                if (tag == null)
                {
                    return NotFound();
                }

                _unitOfWork.Tags.Delete(tag);
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
