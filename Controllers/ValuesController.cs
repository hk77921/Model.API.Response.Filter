using API.Response.Filter.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Response.Filter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class ValuesController : ControllerBase
    {
      //  public AuthMiddleware AuthMiddleware { get; }

        public ValuesController()
        {
            
        }

       
        
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
          
            

            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


  
}
