using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportStore.Api.Models;

namespace SportStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        [HttpGet("cart")]
        public IActionResult GetCart()
        {
            return Ok(HttpContext.Session.GetString("cart"));
        }

        [HttpPost("cart")]
        public void StoreCart([FromBody] ProductSelection[] selections)
        {
            var jsonData = JsonConvert.SerializeObject(selections);
            
            HttpContext.Session.SetString("cart", jsonData);
        }
    }
}