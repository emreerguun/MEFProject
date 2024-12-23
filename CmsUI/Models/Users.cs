using Microsoft.AspNetCore.Identity;

namespace CmsUI.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }

    }
}
