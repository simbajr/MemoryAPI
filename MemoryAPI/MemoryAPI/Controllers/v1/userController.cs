using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemoryAPI.Controllers.v1
{
    public class userController : ApiController
    {
        // /api/v1/user/<username>/media-objects
        // GET: Returns a list of media objects for the user. 
        public string Get(String username)
        {
            return "value";
        }

        // /api/v1/user/<username>/media
        // POST: Create new media file. In body must be either "sound-file", "video-file" or "picture-file".
        public void Post([FromBody]string value)
        {

        }

        // /api/v1/user/<username>
        // GET: Returns the user with username <username>.
        public string Get(String username)
        {
            return "value";
        }

        // DELETE: Deletes everything related to the user with username <username>.
        public void Delete(String username)
        {

        }

        // /api/v1/user/<username>/friends
        // GET: Returns a list of friends the user with username <username> has.
        public string Get(String username)
        {
            return "value";
        }

        // POST: Adds a user as a friend to the user with username <username>.
        public void Post([FromBody]string value)
        {

        }

        // /api/v1/user/<username>/media
        // GET: Returns a list of media the user with username <username> has.
        public string Get(String username)
        {
            return "value";
        }


        // /api/v1/user/<username>/friends/<friendsUsername>
        // DELETE: Removes the user with username <friendsUsername> as a friend to the user with username <username>.
        public void Delete(String username, String friendsUsername)
        {

        }



    }
}
