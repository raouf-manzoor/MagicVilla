using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Extentions;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repository
{
    public class UserRepository : IUserRepository
    {
        #region Fields
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _secretKey;

        #endregion

        #region Ctor
        public UserRepository(ApplicationDbContext db,
            IMapper mapper,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _secretKey = configuration.RetrieveJwtSecret();
            _roleManager = roleManager;
        }
        #endregion

        #region Unique User Identification
        public async Task<bool> IsUniqueUser(string username)
        {

            return !await _db
                .ApplicationUsers
                .AnyAsync(e => e.UserName.ToLower() == username.ToLower());

        }
        #endregion

        #region Login
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(e =>
           e.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            //e.UserName.Equals(loginRequestDTO.UserName, StringComparison.OrdinalIgnoreCase)
            //);

            // Validating password

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            // if user is present generate JWT Token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO response = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                Role = roles.FirstOrDefault()
            };

            return response;
        }
        #endregion

        #region Register
        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {

            ApplicationUser userToRegister = new()
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
                Name = registerationRequestDTO.Name,
            };

            try
            {
                var result = await _userManager
                    .CreateAsync(userToRegister, registerationRequestDTO.Password);

                if (result.Succeeded)
                {
                    // This will add roles in the DB.
                    // This code should be move in the startup and it should seed 
                    // the roles it they not exist in the database.
                    // Currently adding this code here just for testing purpose.

                    var rolesNotExist =!await _roleManager.RoleExistsAsync("admin");

                    if(rolesNotExist)
                    {
                       await _roleManager.CreateAsync(new IdentityRole("admin"));
                       await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }

                    await _userManager.AddToRoleAsync(userToRegister, "admin");

                    var userToReturn = await _db
                                     .ApplicationUsers
                                     .FirstOrDefaultAsync(e => e.UserName == registerationRequestDTO.UserName);

                    return _mapper.Map<UserDTO>(userToReturn);
                }
            }
            catch (Exception ex) { }
           
            return new UserDTO();
        }
        #endregion

    }
}
