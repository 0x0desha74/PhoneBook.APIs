
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.APIs.Extensions;
using PhoneBook.APIs.Helpers;
using PhoneBook.APIs.Middlewares;
using PhoneBook.Core.Entities.Identity;
using PhoneBook.Core.Repositories;
using PhoneBook.Repository.Data;
using PhoneBook.Repository.Identity;
using PhoneBook.Repository.Repositories;
using System.Threading.Tasks;

namespace PhoneBook.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddApplicationServices();
            builder.Services.AddSwaggerServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String Not Found");
                options.UseNpgsql(connectionString);
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("IdentityConnection") ?? throw new InvalidOperationException("Connection String Not Found");
                options.UseNpgsql(connectionString);
            });
            var app = builder.Build();



            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                //Apply ApplicationDbContext Migrations
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                await ApplicationDbContextSeed.SeedData(context);

                //Apply ApplicationIdentityDbContext Migrations
                var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityContext.Database.MigrateAsync();

                //Seeding User Data
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.UserSeedAsync(userManager);

                //Seeding Role Data
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppIdentityDbContextSeed.RoleSeedAsync(roleManager);

            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred During Migration");
            }


            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.MapControllers();

            app.Run();
        }
    }
}
