using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApplication.Application.Contracts;
using MyApplication.Application.Models.Authentication;
using MyApplication.Identity.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApplication.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _rolesManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings, RoleManager<IdentityRole> rolesManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _rolesManager = rolesManager;
            _configuration = configuration;


        }
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse authenticationResponse = new();
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new Exception($"User with {request.Email} not found.");
            var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: user.LockoutEnabled);

            //if (!result.Succeeded)
            //{
            //    //authenticationResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    //authenticationResponse.Message = $"Credentials for '{request.Email} aren't valid'.";
            //    return authenticationResponse;
            //}
            //else if (!user.LockoutEnabled)
            //{
            //    //authenticationResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    //authenticationResponse.Message = $"User is InActive Please Contact to Administrator";
            //    return authenticationResponse;
            //}
            if (!result.Succeeded)
            {
                throw new Exception($"Credentials for '{request.Email} aren't valid'.");
            }
            else if (!user.LockoutEnabled)
            {
                throw new Exception($"User is InActive Please Contact to Administrator");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            
            authenticationResponse.Id = user.Id;
            authenticationResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationResponse.Email = user.Email!;
            authenticationResponse.UserName = user.UserName!;

            return authenticationResponse;
        }

        public async Task<object> ForgetPasswordAsync(string emailaddress)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(new ApplicationUser());
            return "";
            //var user = await _userManager.FindByEmailAsync(emailaddress);
            //if (user != null)
            //{
            //    Email email = new();
            //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //    var resetLink = GenerateResetPasswordLink(user.Id, token);
            //    email.Body = resetLink;
            //    email.Subject = "ForgetPassword";
            //    email.To = user.Email;
            //    await _sendEmail.SendEmailAsync(email);
            //    return new { Message = "Password reset link sent successfully", Link = resetLink };

            //}
            //throw new Exception("User not found with the provided email address");
        }

        public async Task<object> ResetPasswordAsync(PasswordResetRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found with the provided email address");
            }
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (result.Succeeded)
            {
                // Password reset successful
                return new { Message = "Password reset successfully" };
            }
            return result.Errors;
        }

        //private string GenerateResetPasswordLink(string userId, string token)
        //{

        //    var resetLink = $"{_configuration["FrontEndBaseUrl"]}?userId={userId}&token={token}";

        //    return resetLink;

        //}

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null)
            {
                throw new Exception($"Username '{request.UserName}' already exists.");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return new RegistrationResponse() { UserId = user.Id };
                }
                else
                {
                    throw new Exception($"{result.Errors}");
                }
            }
            else
            {
                throw new Exception($"Email {request.Email} already exists.");
            }
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public async Task<object> CreateRole(string roleName)
        {
            var roleExist = await _rolesManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                try
                {
                    var role = new IdentityRole { Name = roleName };
                    var roleResult = await _rolesManager.CreateAsync(role);
                    return roleResult;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            return "Role already exist.";
        }
        public async Task<object> GetAllUsers()
        {
            try
            {
                List<ApplicationUserWithRoles> applicationUserWithRolesList = new List<ApplicationUserWithRoles>();

                List<ApplicationUser> users = await _userManager.Users.ToListAsync();
                users.Select(u => { u.PasswordHash = null; u.SecurityStamp = null; u.ConcurrencyStamp = null; return u; }).ToList();

                foreach (var user in users)
                {
                    List<IdentityRole> roles = new List<IdentityRole>();
                    foreach (IdentityRole role in _rolesManager.Roles.ToList())
                    {
                        var isUserInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                        var userSpecificRoles = await _userManager.GetRolesAsync(user);
                        if (isUserInRole.Where(u => u.UserName == user.UserName).ToList().Count() > 0)
                        {
                            roles.Add(role);
                        }
                    }
                    applicationUserWithRolesList.Add(new ApplicationUserWithRoles { applicationUser = user, roles = roles });
                }
                return applicationUserWithRolesList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<object> ActivateInactiveUsers(string username, bool isActive)
        {
            try
            {
                ApplicationUser existingUser = (await _userManager.FindByNameAsync(username))!;
                if (existingUser != null)
                {
                    return await _userManager.SetLockoutEnabledAsync(existingUser, isActive);
                }
                else
                {
                    throw new Exception($"Username '{username}' not exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
