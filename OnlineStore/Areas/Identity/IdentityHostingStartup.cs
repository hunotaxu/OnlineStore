using DAL.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(OnlineStore.Areas.Identity.IdentityHostingStartup))]
namespace OnlineStore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                //services.AddDbContext<OnlineStoreDbContext>(options =>
                //    options.UseSqlServer(
                //        context.Configuration.GetConnectionString("xuhustoredb")));

                //services.AddDefaultIdentity<OnlineStoreUser>()
                //    .AddEntityFrameworkStores<OnlineStoreDbContext>();
            });
        }
    }
}