using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class ManageScopeController : Controller
    {

        private readonly ConfigurationDbContext _confContext;

        public ManageScopeController(ConfigurationDbContext confContext)
        {
            _confContext = confContext;
        }

        public IActionResult AddScope()
        {
            var scope = new ApiScope { Name = "api_meteo_scope", Enabled = true };
            _confContext.ApiScopes.Add(scope.ToEntity());
            _confContext.SaveChanges();

            return Ok();
        }

        public IActionResult AddGeneralScope()
        {
            var ir = new IdentityResources.OpenId();
            var ir2 = new IdentityResources.Profile();
            _confContext.IdentityResources.Add(ir.ToEntity());
            _confContext.IdentityResources.Add(ir2.ToEntity());
            _confContext.SaveChanges();

            return Ok();
        }
    }
}
