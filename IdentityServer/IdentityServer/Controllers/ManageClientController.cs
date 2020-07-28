using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class ManageClientController : Controller
    {

        private readonly ConfigurationDbContext _confContext;

        public ManageClientController(ConfigurationDbContext confContext)
        {
            _confContext = confContext;
        }

        public IActionResult AddClient()
        {
            IdentityServer4.Models.Client client = new IdentityServer4.Models.Client();

            client.ClientId = "mobileapp";
            client.ClientSecrets = new List<IdentityServer4.Models.Secret> { new IdentityServer4.Models.Secret("cleduclient".Sha256()) };

            client.AllowedScopes = new List<string> { "api_meteo_scope" };

            _confContext.Clients.Add(client.ToEntity());
            _confContext.SaveChanges();
            return Ok();
        }

        public IActionResult UpdateClient()
        {

            var client = _confContext.Clients.Find(1);
            /*var model = client.ToModel();

            model.AllowedGrantTypes = GrantTypes.ClientCredentials;

            client = model.ToEntity();*/
            
            client.AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ClientCredentials } };
            _confContext.Update(client);
            _confContext.SaveChanges();

            return Ok();
        }

        public IActionResult AddClientProfile()
        {
            IdentityServer4.Models.Client client = new IdentityServer4.Models.Client();

            client.ClientId = "mvcapp";
            client.ClientSecrets = new List<IdentityServer4.Models.Secret> { new IdentityServer4.Models.Secret("cleduclient".Sha256()) };

            client.AllowedScopes = new List<string> { "api_meteo_scope", IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile };

            client.AllowedGrantTypes = GrantTypes.Code;

            // URLS de redirection =>>> https://oauth.pstmn.io/v1/callback

            _confContext.Clients.Add(client.ToEntity());
            _confContext.SaveChanges();
            return Ok();
        }
    }
}
