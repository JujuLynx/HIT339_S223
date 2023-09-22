using Microsoft.AspNetCore.Identity;

namespace e_corp.Areas.DATA_2
{
    public class AppUser: IdentityUser
    {
        internal string FirstName;
        internal string LastName;

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }
    }
}
