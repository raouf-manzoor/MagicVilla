using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Extentions;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
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
        private readonly string _secretKey;

        #endregion

        #region Ctor
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _secretKey = configuration.RetrieveJwtSecret();
        }
        #endregion

        #region Unique User Identification
        public async Task<bool> IsUniqueUser(string username)
        {
            return !await _db.LocalUsers.AnyAsync(e => e.UserName == username);

        }
        #endregion

        #region Login
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _db.LocalUsers.FirstOrDefaultAsync(e =>
            e.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            &&
            e.Password == loginRequestDTO.Password
            );

            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            // if user is present generate JWT Token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO response = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return response;
        }
        #endregion

        #region Register
        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            LocalUser userToRegister = _mapper.Map<LocalUser>(registerationRequestDTO);

            await _db.LocalUsers.AddAsync(userToRegister);
            await _db.SaveChangesAsync();

            userToRegister.Password = "";
            return userToRegister;
        }
        #endregion

    }
}
