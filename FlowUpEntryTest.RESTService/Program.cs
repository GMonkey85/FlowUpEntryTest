namespace FlowUpEntryTestWebApp
{
    //copied from sample code on https://learn.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-5.0
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}