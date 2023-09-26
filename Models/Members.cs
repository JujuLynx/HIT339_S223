
namespace e_corp.Models
{
    public class UserWithoutRolesViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        // Add any other relevant properties that you may want to display or process in the view
    }

    public class UsersWithoutRolesListViewModel
    {
        public List<UserWithoutRolesViewModel> UsersWithoutRoles { get; set; } = new List<UserWithoutRolesViewModel>();
    }

}

