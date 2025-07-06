using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApplication.Application.Contracts;
using MyApplication.Application.Models.Authentication;

namespace MyApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UserAccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            try
            {
                return Ok(await _authenticationService.AuthenticateAsync(request));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }        
        }

        [HttpPost("register")]
        //[Authorize] //(Roles="Admin")
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        {

            return Ok(await _authenticationService.RegisterAsync(request));
        }

        [HttpPost("forgetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationResponse>> ForgetPasswordAsync(string emailaddress)
        {

            return Ok(await _authenticationService.ForgetPasswordAsync(emailaddress));
        }
        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationResponse>> ResetPasswordAsync([FromBody] PasswordResetRequest passwordResetRequest)
        {

            return Ok(await _authenticationService.ResetPasswordAsync(passwordResetRequest));
        }


        [HttpPost("CreateRole")]
        [Authorize] //(Roles = "Admin")
        public async Task<object> CreateRole(string roleName)
        {

            return await _authenticationService.CreateRole(roleName);
        }

        [HttpGet("getallusers")]
        [Authorize] //(Roles = "Admin")
        public async Task<object> GetAllUsers()
        {
            return await _authenticationService.GetAllUsers();
        }

        [Authorize] //(Roles = "Admin")
        [HttpPut("ActivateInactiveUsers")]
        public async Task<object> ActivateInactiveUsers(string username, bool isActive)
        {
            return await _authenticationService.ActivateInactiveUsers(username, isActive);
        }
    }
}
