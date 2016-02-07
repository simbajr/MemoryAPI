using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemoryAPI.Controllers.v1
{
    public class mediaController : ApiController
    {


        // DELETE api/media/5
        public void Delete(int id)
        {
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;
            string username = identity.Claims.ElementAt(0).Value;

            using (var db = new MemoryDB())
            {
                var media = (from m in db.Media where m.id == id select m).SingleOrDefault();
                var user = (from u in db.User where u.id == media.user.id select u).SingleOrDefault();

                if (media != null && user != null)
                {
                    try
                    {
                        db.Media.Remove(media);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        var responseErrorMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                        throw new HttpResponseException(responseErrorMsg);
                    }
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

            }
        }
    }
}
