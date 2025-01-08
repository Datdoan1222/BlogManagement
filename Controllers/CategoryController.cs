using AutoMapper;
using BlogManagement.Contracts;
using BlogManagement.Data;
using BlogManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("user/manager/category")]

    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;


        public CategoryController(IMapper mapper, ITokenService tokenService,
            IConfiguration config, IUnitOfWork unitOfWork)
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

            var post = await _unitOfWork.Categories.FindAll(c => c.ParentId == null); 
            var model = _mapper.Map<List<Category>, List<CategoryVM>>(post.ToList());
            return View(model);
        }

        //GET: CategoryController/Details/5
        [Route("details")]
        public async Task<IActionResult> Details(int id)
        {

            var existsId = await _unitOfWork.Categories.IsExists(c => c.Id == id);
            if (!existsId)
            {
                return NotFound();
            }

            //var cate = await _repo.FindById(id);
            var cate = await _unitOfWork.Categories.Find(c => c.Id == id);

            //find childcate
            var cateChildList = await _unitOfWork.Categories.FindAll(c => c.ParentId == id);

            var model = _mapper.Map<CategoryVM>(cate);
            var catealllist = new ShowAllChildCategoryVM
            {
                Id = model.Id,
                ParentId = model.ParentId,
                MetaTitle = model.MetaTitle,
                Title = model.Title,
                Slug = model.Slug,
                Content = model.Content,
                CategoryChild = cateChildList
            };

            return View(catealllist);
        }

        //GET: CategoryController/Create
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
        public async Task<IActionResult> Create(CategoryVM model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);

                }
                //get userid to post 
                var category = _mapper.Map<Category>(model);

                //check create is success 
                await _unitOfWork.Categories.Create(category);
                await _unitOfWork.Save();


                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View();
            }
        }
        // GET: CategoryController/Create
        [Route("create/createchild")]
        public async Task<IActionResult> CreateChild()
        {

            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }


            var cateList = await _unitOfWork.Categories.FindAll(c => c.ParentId == null);
            var cateparentItem = cateList.Select(q => new SelectListItem
            {
                Text = q.Title,
                Value = q.Id.ToString()

            });

            var model = new CategoryChildVM
            {
                CategoryPatens = cateparentItem,
            };

            return View(model);

        }

        // POST: CategoryController/Create
        [Route("create/createchild")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChild(CategoryChildVM category)
        {
            try
            {
                var cateList = await _unitOfWork.Categories.FindAll(c => c.ParentId == null);
                var cateParentSelectList = cateList.Select(q => new SelectListItem
                {
                    Text = q.Title,
                    Value = q.Id.ToString()

                });
                category.CategoryPatens = cateParentSelectList;
                if (!ModelState.IsValid)
                {
                    return View(category);

                }

                var cateListVM = new CategoryVM
                {
                    ParentId = category.ParentId,
                    Title = category.Title,
                    MetaTitle = category.MetaTitle,
                    Slug = category.Slug,
                    Content = category.Content
                };

                //get userid to post 
                var model = _mapper.Map<Category>(cateListVM);

                //check create is success 
                await _unitOfWork.Categories.Create(model);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(category);
            }
        }
        // GET: CategoryController/Edit/5
        [Route("edit")]

        public async Task<IActionResult> Edit(int id)
        {
            var existsId = await _unitOfWork.Categories.IsExists(p => p.Id == id);
            if (!existsId)
            {
                return NotFound();
            }

            var categoryList = await _unitOfWork.Categories.Find(c => c.Id == id);
            var model = _mapper.Map<CategoryVM>(categoryList);

            return View(model);
        }

        // POST: CategoryController/Edit/5
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);

                }

                var category = _mapper.Map<Category>(model);

                _unitOfWork.Categories.Update(category);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            //var cate = await _repo.FindById(id);
            var category = await _unitOfWork.Categories.Find(expression: c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // POST: CategoryController/Delete/5
        [Route("delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CategoryVM model)
        {
            try
            {
                var category = await _unitOfWork.Categories.Find(expression: c => c.Id == id);
                if (category == null)
                {
                    return NotFound();
                }

                _unitOfWork.Categories.Delete(category);
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
