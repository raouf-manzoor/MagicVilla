namespace MagicVilla_Web.Models.Dto
{
    public class UserDTO
    {
        // Convert this to string as we are using
        // .Net Identity and there Id is string.
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        // Removing Role as we can get it through Token.

        // public string Role { get; set; }
    }
}
