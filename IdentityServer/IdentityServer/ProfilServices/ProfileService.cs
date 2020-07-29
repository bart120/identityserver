using IdentityModel;
using IdentityServer.Data;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.ProfilServices
{
    public class ProfileService : IProfileService
    {
        private readonly AuthenticationDbContext _authContext;

        public ProfileService(AuthenticationDbContext authContext)
        {
            _authContext = authContext;
        }

        private async Task<IEnumerable<Claim>> GetClaimsFromUser(ProfileDataRequestContext context)
        {
            var idUser = Convert.ToInt32(context.Subject.Claims.SingleOrDefault(x => x.Type == "sub").Value);
            var user = await _authContext.Users.SingleOrDefaultAsync(x => x.ID == idUser);
            if(user == null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.ID.ToString())
            };

            //claims.Add(new Claim("mobileapp", "true"));

            claims.Add(new Claim("contract", "5485HJ5"));
            claims.Add(new Claim(JwtClaimTypes.Role, "AUTRE"));

            return claims;

            
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = await GetClaimsFromUser(context);

            context.IssuedClaims.AddRange(claims);
        }

        

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }

        
    }
}
