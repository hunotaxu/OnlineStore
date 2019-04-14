using System;
using DAL.EF;
using DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineStore.Middleware;
using OnlineStore.Services;

namespace OnlineStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(69);
            });
            services.AddMemoryCache();
            // using Microsoft.AspNetCore.Identity.UI.Services;
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddSingleton(_ => Configuration);
            services.AddDbContext<OnlineStoreDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("OnlineStoreContextConnection")).UseLazyLoadingProxies());
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<OnlineStoreDbContext>()
                .AddDefaultTokenProviders();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/_Home/Index", "");
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // using Microsoft.AspNetCore.Identity.UI.Services;
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserDecentralizationRepository, UserDecentralizationRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartDetailRepository, CartDetailRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ILineItemRepository, LineItemRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/log-{Date}.txt");
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
            //app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            ////serve up files from the node_modules folder
            app.UseNodeModules(env.ContentRootPath);
            //app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc();
        }
    }
}