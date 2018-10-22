using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace OnlineStore.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        // This is extension method for IApplicationBuilder, so use this keyword
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="root">root of this project</param>
        /// <returns></returns>
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string root)
        {
            // a path to the node_modules folder
            var path = Path.Combine(root, "node_modules");
            var fileProvider = new PhysicalFileProvider(path);
            var options = new StaticFileOptions
            {
                RequestPath = "/node_modules", FileProvider = fileProvider
            };
            app.UseStaticFiles(options);
            return app;
        }
    }
}
