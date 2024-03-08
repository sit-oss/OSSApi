using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OSSApi.Controllers
{
    /// <summary>
    /// Get some values, just an example, no authentication required
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Say Hello
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[] { "Hello", "World" });
        }

        /// <summary>
        /// Get some items
        /// </summary>
        /// <param name="item">can be 'headers'</param>
        /// <returns></returns>
        [HttpGet("{item}")]
        public IActionResult GetItems(string item)
        {
            switch (item)
            {
                case "headers":
                    return new JsonResult(from c in Request.Headers select new { c.Key, c.Value });
                default:
                    return Ok(new[] { "" });
            }
        }
    }
}
