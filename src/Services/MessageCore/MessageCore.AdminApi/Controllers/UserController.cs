
using MessageCore.AdminApi.Model;
using MessageCore.Application.Services;
using MessageCore.Application.Services.Base;
using MessageCore.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MessageCore.AdminApi.Controllers
{
    [ApiController]
    [Route("api/v1/core/Users")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly IPortalService _portalService;

        public UserController(ILogger<UserController> logger, IPortalService portalService)
        {
            _logger = logger;
            _portalService = portalService;
        }

        [HttpOptions("login")]
        [HttpPost("login")]
        public IActionResult Login(LoginParams login)
        {
            var token = _portalService.Login(login.Username, login.Password);
            var result = new LoginResultModel()
            {
                Token = token,
                UserId = "1",
                Role = new RoleInfo() { RoleName = "Super Admin", Value = "super" },
            };
            return Ok(result);

            //if ((login.Username == "employeetest" || login.Username == "employeeTest") && login.Password == "111111")
            //{
            //    result.Role = new RoleInfo() { RoleName = "Super Admin", Value = "super" };
            //    result.UserId = "1";
            //    result.Token = "testToken";
            //    return Ok(result);
            //}
            //else
            //{
            //    throw new MessageCoreInternalException(ErrorCode.StringCode.LoginFail, string.Format(ErrorMessage.LoginFail, login.Username));
            //}
        }

        [HttpGet("logout")]
        public IActionResult Logout(string token)
        {
            _portalService.Logout(token);
            var result = new LoginResultModel();
            return Ok(result);
        }

        [HttpPost("getPermCode")]
        public IActionResult GetPermCode(string token)
        {
            var result = new[] { "1000", "3000", "5000" };
            return Ok(result);
        }

        [HttpGet("getUserInfo")]
        public IActionResult GetUserInfo(string token)
        {
            var info = _portalService.GetUserInfo(token);

            var result = new GetUserInfoModel()
            {
                UserId = "1",
                Username = info.LoginName,
                RealName = info.LoginName,
                Avatar = "https://q1.qlogo.cn/g?b=qq&nk=190848757&s=640",
                Desc = "manager",
                Roles = new[] { new RoleInfo() { RoleName = "Super Admin", Value = "super" } },
            };
            return Ok(result);

            //var result = new GetUserInfoModel()
            //{
            //    UserId = "1",
            //    Username = "employeetest",
            //    RealName = "employeetest",
            //    Avatar = "https://q1.qlogo.cn/g?b=qq&nk=190848757&s=640",
            //    Desc = "manager",
            //    Roles = new[] { new RoleInfo() { RoleName = "Super Admin", Value = "super" } },

            //};
            //return Ok(result);
        }
    }
}
