
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region additional namespaces
using Chinook.Data.Entities.Security;
using Microsoft.AspNet.Identity;
using ChinookSystem.DAL.Security;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace ChinookSystem.BLL.Security
{
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
        }
    }
}
