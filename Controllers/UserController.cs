using AutoMapper;
using BlogManagement.Contracts;
using BlogManagement.Data;
using BlogManagement.Models;
using BlogManagement.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [AllowAnonymous, Route("user")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private string generatedToken = null;

        public UserController(IUnitOfWork unitOfWork, IConfiguration config, ITokenService tokenService,
            IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _config = config;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Route("manageruser")]
        public async Task<IActionResult> ManagerUser()
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }
            var userList = await _unitOfWork.Users.FindAll();

            var model = _mapper.Map<List<User>, List<ShowUser>>(userList.ToList());
            return View(model);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userModel)
        {
            //genarate passwordhash and find user

            var passwordHash = _userRepository.GetPasswordHash(userModel.PasswordHash);
            userModel.PasswordHash = passwordHash;

            if (string.IsNullOrEmpty(userModel.Email) || string.IsNullOrEmpty(passwordHash))
            {
                ModelState.AddModelError("", "Please enter a Username and Password...");
                return View(userModel);
            }
            //get user
            var validUser = await _unitOfWork.Users.Find(x => x.Email.ToLower() == userModel.Email.ToLower()
                    && x.PasswordHash == userModel.PasswordHash);


            if (validUser != null)
            {
                generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), validUser);
                if (generatedToken != null && validUser.Role == "Admin")
                {

                    HttpContext.Session.SetString("Token", generatedToken);
                    TempData["SuccessMessage"] = "1";
                    return RedirectToAction(nameof(ManagerPage));

                }
                else if (generatedToken != null && validUser.Role == "Blogger")
                {
                    HttpContext.Session.SetString("Token", generatedToken);
                    return RedirectToAction("Index", "Home");
                }
                else if (generatedToken != null && validUser.Role == "Guest")
                {
                    HttpContext.Session.SetString("Token", generatedToken);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Can't generate token...");
                    return View(userModel);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Password not correct...");
                return View("Index", userModel);
            }
        }

        [AllowAnonymous]
        [Route("register")]
        public IActionResult RegisterAccount()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccount(RegisterVM userModel)
        {
            try
            {
                //check mail
                var existsEmail = await _unitOfWork.Users.IsExists(u => u.Email == userModel.Email);
                if (existsEmail == true)
                {
                    ModelState.AddModelError("", "Email already exists, please choose another email...");
                    return View(userModel);
                }
                //check mobile
                var existsMobile = await _unitOfWork.Users.IsExists(u => u.Mobile == userModel.Mobile);
                if (existsMobile == true)
                {
                    ModelState.AddModelError("", "Mobile already exists, please choose another mobile...");
                    return View(userModel);
                }
                var passwordHash = _userRepository.GetPasswordHash(userModel.PasswordHash);
                userModel.PasswordHash = passwordHash;
                var model = _mapper.Map<User>(userModel);

                //get name from fullname
                var fullname = userModel.FullName.Split(' ');

                for (int i = 0; i < fullname.Length; i++)
                {
                    if (i == 0)
                    {
                        model.Firstname = fullname[i];
                    }
                    else if (i == 1)
                    {
                        model.Middlename = fullname[i];
                    }
                    else
                    {
                        model.Lastname = fullname[i];
                    }

                }
                model.RegisteredAt = DateTime.Now;
                model.Role = "Guest";
                model.ImagePath = "/Images/images.png";
                //var isSucces = await _userRepository.PostUserRegister(model);
                await _unitOfWork.Users.Create(model);
                await _unitOfWork.Save();

                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(userModel);
            }

        }


        [Authorize(Roles = "Admin")]
        [Route("manager")]
        [HttpGet]
        public IActionResult ManagerPage()
        {

            string token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return (RedirectToAction("Index"));
            }
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index"));
            }

            return View();
        }

        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            string token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return (RedirectToAction("Index", "Home"));

            }
            HttpContext.Session.Clear();
            return (RedirectToAction("Index", "Home"));


        }
        [Route("error")]

        public IActionResult Error()
        {
            ViewBag.Message = "An error occured...";
            return View();
        }


        [Route("googlelogin")]
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        [Route("googleresponse")]
        public async Task<IActionResult> GoogleResponse(PostLoginGoogleandFacebookVM user)
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var claims = result.Principal.Identities
                    .FirstOrDefault().Claims.Select(claim => new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value,
                    });
                //get info from claims to add in table user
                var email = claims.Where(n => n.Type == ClaimTypes.Email).Select(n => n.Value).FirstOrDefault();
                var nameIdentifier = claims.Where(n => n.Type == ClaimTypes.NameIdentifier).Select(n => n.Value).FirstOrDefault();
                var name = User.Identity.Name;
                var image = claims.Where(n => n.Type == "urn:google:picture").Select(n => n.Value).FirstOrDefault();

                //split to get name 
                var fullname = name.Split(' ');

                //add in user PostLoginGoogleandFacebookVM
                for (int i = 0; i < fullname.Length; i++)
                {
                    if (i == 0)
                    {
                        user.Firstname = fullname[i];
                    }
                    else if (i == 1)
                    {
                        user.Middlename = fullname[i];
                    }
                    else
                    {
                        user.Lastname = fullname[i];
                    }


                }
                user.Email = email;
                user.NameIdentifier = nameIdentifier;
                user.ImagePath = image;
                user.Role = "Guest";
                //check mail 

                //add PostLoginGoogleandFacebookVM in User to post in db
                var userLogin = await _unitOfWork.Users.Find(x => x.Email.ToLower() == user.Email.ToLower()
                        && x.NameIdentifier == user.NameIdentifier);
                var usernameIdentifier = await _unitOfWork.Users.IsExists(x => x.NameIdentifier == nameIdentifier);
                var isExistsEmail = await _unitOfWork.Users.IsExists(x => x.Email == email);

                if (isExistsEmail && !usernameIdentifier)
                {
                    await HttpContext.SignOutAsync("MultiScheme");
                    ModelState.AddModelError("", "Email is Existed in account");
                    return RedirectToAction("Index", "Home");
                }
                if (userLogin == null)
                {
                    var userdb = _mapper.Map<User>(user);
                    userdb.RegisteredAt = DateTime.Now;
                    userdb.Mobile = "google";

                    //check create is success 
                    //var isSucces = await _userRepository.PostUserLogin(userdb);
                    await _unitOfWork.Users.Create(userdb);
                    await _unitOfWork.Save();


                    if (result != null)
                    {
                        await HttpContext.SignOutAsync("MultiScheme");
                    }

                    //var userlogin1 = await _userRepository.GetUserFBGG(user);
                    var userrecheck = await _unitOfWork.Users.Find(x => x.Email.ToLower() == user.Email.ToLower()
                       && x.NameIdentifier == user.NameIdentifier);
                    //create token new for user 
                    if (userrecheck != null)
                    {
                        generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), userrecheck);
                        if (generatedToken != null && userrecheck.Role == "Admin")
                        {

                            HttpContext.Session.SetString("Token", generatedToken);
                            return RedirectToAction("ManagerPage");
                        }
                        else if (generatedToken != null && userrecheck.Role == "Blogger")
                        {
                            HttpContext.Session.SetString("Token", generatedToken);
                            return RedirectToAction("Index", "Home");
                        }
                        else if (generatedToken != null && userrecheck.Role == "Guest")
                        {
                            HttpContext.Session.SetString("Token", generatedToken);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Can't generate token...");
                            return View(user);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Password not correct...");
                        return View("Index", user);
                    }

                }
                else if (userLogin != null && isExistsEmail && userLogin.Mobile == "google")
                {
                    if (result != null)
                    {
                        await HttpContext.SignOutAsync("MultiScheme");
                    }

                    generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), userLogin);
                    if (generatedToken != null && userLogin.Role == "Admin")
                    {

                        HttpContext.Session.SetString("Token", generatedToken);
                        return RedirectToAction("ManagerPage");
                    }
                    else if (generatedToken != null && userLogin.Role == "Blogger")
                    {
                        HttpContext.Session.SetString("Token", generatedToken);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (generatedToken != null && userLogin.Role == "Guest")
                    {
                        HttpContext.Session.SetString("Token", generatedToken);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't generate token...");
                        return View(user);
                    }

                }
                else
                {
                    await HttpContext.SignOutAsync("MultiScheme");
                    ModelState.AddModelError("", "Email is Existed in account");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View("Index", "User");
            }
        }
        // AccountController.cs

        [Route("facebooklogin")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [Route("facebookresponse")]
        public async Task<IActionResult> FacebookResponse(PostLoginGoogleandFacebookVM user)
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var claims = result.Principal.Identities
                    .FirstOrDefault().Claims.Select(claim => new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value
                    });
                //get info from claims to add in table user
                var email = claims.Where(n => n.Type == ClaimTypes.Email).Select(n => n.Value).FirstOrDefault();
                var nameIdentifier = claims.Where(n => n.Type == ClaimTypes.NameIdentifier).Select(n => n.Value).FirstOrDefault();
                var name = User.Identity.Name;
                var image = claims.Where(n => n.Type == "Picture").Select(n => n.Value).FirstOrDefault();

                //split to get name 
                var fullname = name.Split(' ');

                //add in user PostLoginGoogleandFacebookVM
                for (int i = 0; i < fullname.Length; i++)
                {
                    if (i == 0)
                    {
                        user.Firstname = fullname[i];
                    }
                    else if (i == 1)
                    {
                        user.Middlename = fullname[i];
                    }
                    else
                    {
                        user.Lastname = fullname[i];
                    }


                }
                user.Email = email;
                user.NameIdentifier = nameIdentifier;
                user.ImagePath = image;
                user.Role = "Guest";

                //check mail 

                //add PostLoginGoogleandFacebookVM in User to post in db
                var userLogin = await _unitOfWork.Users.Find(x => x.Email.ToLower() == user.Email.ToLower()
                        && x.NameIdentifier == user.NameIdentifier);
                var usernameIdentifier = await _unitOfWork.Users.IsExists(x => x.NameIdentifier == nameIdentifier);
                var isExistsEmail = await _unitOfWork.Users.IsExists(x => x.Email == email);

                if (isExistsEmail && !usernameIdentifier)
                {
                    await HttpContext.SignOutAsync("MultiScheme");
                    ModelState.AddModelError("", "Email is Existed in account");
                    return RedirectToAction("Index", "Home");
                }
                if (userLogin == null)
                {
                    var userdb = _mapper.Map<User>(user);
                    userdb.RegisteredAt = DateTime.Now;
                    userdb.Mobile = "facebook";

                    //check create is success 
                    await _unitOfWork.Users.Create(userdb);
                    await _unitOfWork.Save();

                    if (result != null)
                    {
                        await HttpContext.SignOutAsync("MultiScheme");
                    }
                    var userrecheck = await _unitOfWork.Users.Find(x => x.Email.ToLower() == user.Email.ToLower()
                       && x.NameIdentifier == user.NameIdentifier);
                    //create token new for user 
                    if (userrecheck != null)
                    {
                        generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), userrecheck);
                        if (generatedToken != null && userrecheck.Role == "Admin")
                        {

                            HttpContext.Session.SetString("Token", generatedToken);
                            return RedirectToAction("ManagerPage");
                        }
                        else if (generatedToken != null && userrecheck.Role == "Blogger")
                        {
                            HttpContext.Session.SetString("Token", generatedToken);
                            return RedirectToAction("Index", "Home");
                        }
                        else if (generatedToken != null && userrecheck.Role == "Guest")
                        {
                            HttpContext.Session.SetString("Token", generatedToken);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Can't generate token...");
                            return View(user);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Password not correct...");
                        return View("Index", user);
                    }

                }
                else if (userLogin != null && isExistsEmail && userLogin.Mobile == "facebook")
                {
                    if (result != null)
                    {
                        await HttpContext.SignOutAsync("MultiScheme");
                    }

                    generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), userLogin);
                    if (generatedToken != null && userLogin.Role == "Admin")
                    {

                        HttpContext.Session.SetString("Token", generatedToken);
                        return RedirectToAction("ManagerPage");
                    }
                    else if (generatedToken != null && userLogin.Role == "Blogger")
                    {
                        HttpContext.Session.SetString("Token", generatedToken);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (generatedToken != null && userLogin.Role == "Guest")
                    {
                        HttpContext.Session.SetString("Token", generatedToken);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't generate token...");
                        return View(user);
                    }

                }
                else
                {
                    await HttpContext.SignOutAsync("MultiScheme");
                    ModelState.AddModelError("", "Email is Existed in account");
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View("Index", "User");
            }


        }

        [Route("user/manager/edit")]
        public async Task<IActionResult> Edit(long id)
        {
            string token = HttpContext.Session.GetString("Token");
            if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index", "Home"));
            }
            var userId = int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);
            var user = await _unitOfWork.Users.Find(u => u.Id == userId);
            var userVM = _mapper.Map<UserVM>(user);

            return View(userVM);

        }

        // POST: PostController/Edit/5
        [Route("user/manager/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserVM model)
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
                    var uploadImage = new ImageUploadUsers();
                    uploadImage.UpLoandImageFromFile(model);

                }

                var user = _mapper.Map<User>(model);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.Save();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

    }
}

