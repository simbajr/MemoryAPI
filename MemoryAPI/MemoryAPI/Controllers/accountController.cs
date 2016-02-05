using MemoryAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;



namespace MemoryAPI.Controllers
{
    
    public class accountController : ApiController
    {
       /* // GET api/account
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/account/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/account
        public void Post([FromBody]string value)
        {
        }

        // PUT api/account/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/account/5
        public void Delete(int id)
        {
        }*/

        private AuthRepository _repo = null;
      
        public accountController()
        {
           _repo = new AuthRepository();
        }

        //POST api/account/Register
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(Account account)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(account);

            IHttpActionResult errorResult = GetErrorResult(result);

            if(errorResult!=null)
            {
                return errorResult;
            }

            return OK();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _repo.Dispose();
            }
            
            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if(result==null)
            {
                return InternalServerError();
            }
            if(!result.Succeeded)
            {
                if(result.Errors!=null)
                {
                    foreach(string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if(ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }
            return null;
        }
    }
}
