using MessagingService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public AppDbContext DbContext { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("MessagingServiceTestDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            DbContext.Database.EnsureCreated();
        });
    }
}
