namespace MagicVilla_API.Models.Dto
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }

        public string Token { get; set; }

        // We don't have to pass role explicitly as we get that info from Token.
        //public string Role { get; set; }
    }
}
