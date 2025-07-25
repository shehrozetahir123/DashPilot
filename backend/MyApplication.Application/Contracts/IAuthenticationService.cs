﻿using MyApplication.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApplication.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);

        Task<object> ForgetPasswordAsync(string emailaddress);
        Task<object> ResetPasswordAsync(PasswordResetRequest request);


        Task<object> CreateRole(string roleName);

        Task<object> GetAllUsers();
        Task<object> ActivateInactiveUsers(string username, bool isActive);
    }
}
