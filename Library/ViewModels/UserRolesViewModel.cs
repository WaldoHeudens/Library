using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class UserRolesViewModel
    {
        [Display (Name = "User")]
        public string UserName { get; set; }

        [Display (Name = "Roles")]
        public List<string> Roles { get; set; }
    }
}
