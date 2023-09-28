using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using e_corp.Areas.Identity.Data;

namespace e_corp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("e_corpIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'e_corpIdentityDbContextConnection' not found.");

            builder.Services.AddDbContext<e_corpIdentityDbContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<e_corpIdentityDbContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Training}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager =
                    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Coach" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string adminEmail = "admin@admin.com";
                string adminPassword = "Admin123!";

                string coachMattEmail = "matt@coach.com";
                string coachMattPassword = "Matt123!";

                string coachKyleEmail = "kyle@coach.com";
                string coachKylePassword = "Kyle123!";

                string coachMichaelEmail = "michael@coach.com";
                string coachMichaelPassword = "Michael123!";

                string coachMeherinaEmail = "meherina@coach.com";
                string coachMeherinaPassword = "Meherina123!";

                string memberEmail = "member@member.com";
                string memberPassword = "Member123!";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = adminEmail;
                    user.Email = adminEmail;

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, "Admin");
                }

                if (await userManager.FindByEmailAsync(coachMattEmail) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = coachMattEmail;
                    user.Email = coachMattEmail;

                    await userManager.CreateAsync(user, coachMattPassword);

                    await userManager.AddToRoleAsync(user, "Coach");
                }

                if (await userManager.FindByEmailAsync(coachKyleEmail) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = coachKyleEmail;
                    user.Email = coachKyleEmail;

                    await userManager.CreateAsync(user, coachKylePassword);

                    await userManager.AddToRoleAsync(user, "Coach");
                }

                if (await userManager.FindByEmailAsync(coachMichaelEmail) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = coachMichaelEmail;
                    user.Email = coachMichaelEmail;

                    await userManager.CreateAsync(user, coachMichaelPassword);

                    await userManager.AddToRoleAsync(user, "Coach");
                }

                if (await userManager.FindByEmailAsync(coachMeherinaEmail) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = coachMeherinaEmail;
                    user.Email = coachMeherinaEmail;

                    await userManager.CreateAsync(user, coachMeherinaPassword);

                    await userManager.AddToRoleAsync(user, "Coach");
                }

                if (await userManager.FindByEmailAsync(memberEmail) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = memberEmail;
                    user.Email = memberEmail;

                    await userManager.CreateAsync(user, memberPassword);
                }

            }

            app.Run();
        }
    }

}



