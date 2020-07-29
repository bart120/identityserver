using IdentityServer.Data;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Validators
{
    public class ROClientValidator : IResourceOwnerPasswordValidator{

        private readonly AuthenticationDbContext _authContext;

        public ROClientValidator(AuthenticationDbContext authContext)
        {
            _authContext = authContext;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _authContext.Users.SingleOrDefault(x => x.Login == context.UserName && x.Password == context.Password.Sha256());
            if (user != null)
            {

                context.Result = new GrantValidationResult(user.ID.ToString(), "password", null, "local", null);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Login ou mot de passe invalide", null);
            }

            return Task.FromResult(context.Result);
        }
    }
}
