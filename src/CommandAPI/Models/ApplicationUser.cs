using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CommandAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }

        // 注意：下面继承IdentityUser的数据要保持统一名字
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    }
}