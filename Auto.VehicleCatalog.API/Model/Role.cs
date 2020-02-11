using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Auto.VehicleCatalog.API.Model
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}