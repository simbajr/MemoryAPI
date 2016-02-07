using MemoryAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MemoryAPI
{
    public class AuthRepository : IDisposable
    {
        private memoryDB _ctx;

        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new memoryDB();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(Account userModel)
        {
            /* IdentityUser user = new IdentityUser
             {
                 UserName = userModel.username
             };

             var result = await _userManager.CreateAsync(user, userModel.password);

             return result;*/

            using (var db = new memoryDB())
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = userModel.username,
                    Email = userModel.email
                };

                var result = await _userManager.CreateAsync(user, userModel.password);

                return result;
            }
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}