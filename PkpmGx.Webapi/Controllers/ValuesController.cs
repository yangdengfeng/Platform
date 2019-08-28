using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PkpmGx.Webapi.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        IRepsitory<Role> roleRep;

        public ValuesController(IRepsitory<Role> roleRep)
        {
            this.roleRep = roleRep;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return roleRep.GetByCondition(r => true).Select(r => r.Code).ToList();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
