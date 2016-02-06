using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemoryAPI.Controllers.v1
{
    public class vacationController : ApiController
    {
        // GET api/vacation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/vacation/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/vacation
        public void Post([FromBody]string value)
        {
        }

        // PUT api/vacation/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/vacation/5
        public void Delete(int id)
        {
        }
    }
}
