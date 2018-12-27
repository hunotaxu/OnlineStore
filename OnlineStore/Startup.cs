using System;
using DAL.EF;
using DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Middleware;

namespace OnlineStore
{
    public class Startup
    {
        IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession();
            services.AddMemoryCache();
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Home/Index", "");
            });
            services.AddSingleton(_ => _configuration);
            services.AddDbContext<OnlineStoreDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("OnlineStore")));

            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserDecentralizationRepository, UserDecentralizationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartDetailRepository, CartDetailRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            app.UseStaticFiles();
            ////serve up files from the node_modules folder
            app.UseNodeModules(env.ContentRootPath);
            //app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc();
        }
    }
}
