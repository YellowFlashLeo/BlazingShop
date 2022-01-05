using Microsoft.AspNetCore.Identity;

namespace BlazingShop.Shared.Modals
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
