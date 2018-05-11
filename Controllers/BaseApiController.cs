using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportStore.Api.Data;
using SportStore.Api.Models;

namespace SportStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        public BaseApiController(DataContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            DbContext = dbContext;
            RoleManager = roleManager;
            UserManager = userManager;
            Configuration = configuration;
        }

        protected DataContext DbContext { get; private set; }
        
        protected RoleManager<IdentityRole> RoleManager { get; private set; }
        
        protected UserManager<ApplicationUser> UserManager { get; private set; }
        
        protected IConfiguration Configuration { get; private set; }
    }
}