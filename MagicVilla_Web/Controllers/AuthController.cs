﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {

        #region Fields
        private readonly IAuthService _authService;
        #endregion

        #region Ctor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Login

        // This method is used to tell HttpContext that the user is signed in.
        // In order to work Authorize attribute on the client side controllers
        private async Task HttpSignIn(LoginResponseDTO loginResponse)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            // We are decrypting the token we receive after login into the system
            // to extract the required role Info.

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }


        [HttpGet]
        public IActionResult Login()
        {
            var loginObj = new LoginRequestDTO();
            return View(loginObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginObj)
        {
            var loginResult = await _authService.LoginAsync<APIResponse>(loginObj);

            if (loginResult != null && loginResult.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<LoginResponseDTO>(loginResult.Result.ToString());

                await HttpSignIn(model);

                HttpContext.Session.SetString(SD.SessionToken, model.Token);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("CustomError", loginResult.ErrorMessages.FirstOrDefault());

            return View(loginObj);
        }
        #endregion

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            var rolesList = new List<SelectListItem>()
            {
                new SelectListItem() {Text=SD.Admin,Value=SD.Admin},
                new SelectListItem() {Text=SD.Customer,Value=SD.Customer}
            };

            ViewBag.RoleList = rolesList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO registerUserObj)
        {
            if (string.IsNullOrEmpty(registerUserObj.Role))
            {
                registerUserObj.Role=SD.Customer;
            }

            var registerResult = await _authService.RegisterAsync<APIResponse>(registerUserObj);
           
            if (registerResult != null && registerResult.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            // To populate roles dropdown if there is any issue while registration.

            var rolesList = new List<SelectListItem>()
            {
                new SelectListItem() {Text=SD.Admin,Value=SD.Admin},
                new SelectListItem() {Text=SD.Customer,Value=SD.Customer}
            };

            ViewBag.RoleList = rolesList;

            return View();
        }

        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
