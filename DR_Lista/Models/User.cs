using Microsoft.AspNetCore.Identity;

namespace DR_Lista.Models
{
    public class User : IdentityUser
    {
        public string NickName { get; set; }
    }
}
