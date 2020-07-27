using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class ManageApiController : Controller
    {
        private readonly ConfigurationDbContext _confContext;

        public ManageApiController(ConfigurationDbContext confContext)
        {
            _confContext = confContext;
        }


        public IActionResult AddApiRessource()
        {
            IdentityServer4.Models.ApiResource api = new IdentityServer4.Models.ApiResource();

            api.Name = "apimeteo";
            api.Description = "description de l'api";
            api.DisplayName = "API meteo";
            api.Enabled = true;


            _confContext.ApiResources.Add(api.ToEntity()) ;
            _confContext.SaveChanges();
            return Ok();
        }
    }
}
