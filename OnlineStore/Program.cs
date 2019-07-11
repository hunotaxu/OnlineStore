using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace OnlineStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        // CreateDefaultBuilder cấu hình Kestrel server như một web server và cho phép tích hợp IIS
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
