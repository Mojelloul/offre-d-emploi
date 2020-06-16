using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication5.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication5.Startup))]
namespace WebApplication5
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if (!roleManager.RoleExists("Admins"))
            {
                role.Name = "Admins";
                roleManager.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Mohamed";
                user.Email = "realmed7@hotmail.fr";
                var Check = userManager.Create(user, "Aaaaaa-7");
                if (Check.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admins");//22
                }
            }
        }
    }
}
