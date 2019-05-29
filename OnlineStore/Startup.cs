using System;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using Newtonsoft.Json;
using OnlineStore.Middleware;
using OnlineStore.Services;
using TimiApp.Dapper.Implementation;
using TimiApp.Dapper.Interfaces;

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
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
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
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddSessionStateTempDataProvider()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Home/Index", "");
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services.AddSession();
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(69);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "OnlineStoreCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.Configure<PasswordHasherOptions>(option =>
            {
                option.IterationCount = 12000;
            });

            // using Microsoft.AspNetCore.Identity.UI.Services;
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            //services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<IUserDecentralizationRepository, UserDecentralizationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartDetailRepository, CartDetailRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IReportService, ReportService>();
            //services.AddScoped<ISettings, PdfSettings>();
            services.AddScoped<IPdfSettings, PdfSettings>();
            //services.AddScoped<INopFileProvider, NopFileProvider>();
            //services.AddScoped<PdfSettings>();
            services.AddScoped<IGoodsReceiptDetailRepository, GoodsReceiptDetailRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IProductImagesRepository, ProductImagesRepository>();
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
            
            app.UseAuthentication();
            ////serve up files from the node_modules folder
            app.UseNodeModules(env.ContentRootPath);
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "MyArea",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}