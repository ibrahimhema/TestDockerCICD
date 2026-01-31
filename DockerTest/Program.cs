using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DockerTest.Data;
using DockerTest.Areas.Identity.Data;
using StackExchange.Redis;
namespace DockerTest
{
    public class Program
    {
        public static void InsertUsers(IServiceCollection services)
        {

          var context=  services.BuildServiceProvider().GetRequiredService<DockerTestContext>();
            context.Database.Migrate();
            context.Users.Add(new DockerTestUser
            {
                Email = "test@test.com",
                UserName = "test",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "123456789",
                NormalizedEmail = "TEST@TEST.COM"
            });
            context.SaveChanges();
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                        var connectionString = builder.Configuration.GetConnectionString("DockerTestContextConnection") ?? throw new InvalidOperationException("Connection string 'DockerTestContextConnection' not found.");

                                    builder.Services.AddDbContext<DockerTestContext>(options =>
                options.UseSqlServer(connectionString));

                                                builder.Services.AddDefaultIdentity<DockerTestUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DockerTestContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration["Redis:Connection"]));
            builder.Services.AddHealthChecks();
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
                        app.UseAuthentication();;

            app.UseAuthorization();
            //InsertUsers(builder.Services);
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHealthChecks("/health");
            app.Run();
        }
    }
}