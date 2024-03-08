using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OSSApi.Controllers
{
    /// <summary>
    /// User controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class MyController : ControllerBase
    {
        private readonly IUserInfoClient _userInfoClient;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userInfoClient"></param>
        public MyController(IUserInfoClient userInfoClient)
        {
            _userInfoClient = userInfoClient;
        }
        /// <summary>
        /// Get the user claims
        /// </summary>
        /// <returns></returns>
        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// Get the user info
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<UserInfo> GetUserInfo()
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            return await _userInfoClient.GetUserInfo(token);
        }
    }
}
