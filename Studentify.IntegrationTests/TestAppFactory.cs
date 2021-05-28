using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Studentify.Data;

namespace Studentify.IntegrationTests
{
    public class TestAppFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<StudentifyDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<StudentifyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<StudentifyDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TestAppFactory<TStartup>>>();

                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
