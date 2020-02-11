using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Auto.VehicleCatalog.API.Model
{
    public class User : IdentityUser<int>
    {
            public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}