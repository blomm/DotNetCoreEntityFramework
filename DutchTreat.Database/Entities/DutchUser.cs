using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Database.Entities{

    public class DutchUser:IdentityUser{
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}