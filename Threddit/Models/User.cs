using Microsoft.AspNetCore.Identity;

namespace Threddit.Models
{
    public class User : IdentityUser
    {
        public int uid { get; set; }
    }
}
