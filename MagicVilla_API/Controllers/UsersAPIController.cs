using MagicVilla_API.Extentions;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    //[Route("api/[controller]")] In that case, Controller Name will automatically populated. 
    // We will avoid that approach, In case of change in controller name. we have to update all
    // the client apps which are consuming that endpoint
    [Route("api/v{version:apiVersion}/UsersAuthAPI")]
    [ApiVersionNeutral] // States that it will remain constant across all the versions
    [ApiController]
    public class UsersAPIController : Controller
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private APIResponse _response;
        #endregion

        #region Ctor
        public UsersAPIController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new();
        }
        #endregion


        #region Login

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestModel)
        {
            var loginResponse = await _userRepository.Login(loginRequestModel);

            if (loginResponse.User == null || loginResponse.Token.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;

            return Ok(_response);
        }
        #endregion

        #region Register

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO registerationRequestModel)
        {
            var isUserNameUnique = await _userRepository.IsUniqueUser(registerationRequestModel.UserName);

            if (!isUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            var user= await _userRepository.Register(registerationRequestModel);

            if(user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            return Ok(_response);
        }

        #endregion
    }
}
