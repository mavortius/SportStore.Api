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
        public IActionResult GetCart() => Ok(HttpContext.Session.Get<ProductSelection[]>("cart"));

        [HttpPost("cart")]
        public void StoreCart([FromBody] ProductSelection[] selections)
            => HttpContext.Session.Set("cart", selections);
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}