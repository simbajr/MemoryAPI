using MemoryAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


//skapad av Simba
namespace MemoryAPI
{
    public class AuthRepository : IDisposable
    {
        private MemoryDB _ctx;

        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new MemoryDB();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(Account userModel)
        {          
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.username
            };

            using (var db = new MemoryDB())
            {
                User newUser = new User();
                List<Media> mediaList = new List<Media>();
                List<User> friendList = new List<User>();
                newUser.friendList = friendList;
                newUser.username = userModel.username;
                newUser.email = userModel.email;                
                newUser.mediaList = mediaList;

                db.User.Add(newUser);
                db.SaveChanges();
            }

            var result = await _userManager.CreateAsync(user, userModel.password);

            return result;
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