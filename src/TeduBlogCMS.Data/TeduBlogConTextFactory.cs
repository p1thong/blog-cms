using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TeduBlogCMS.Data
{
    public class TeduBlogConTextFactory : IDesignTimeDbContextFactory<TeduBlogConText>
    {
        public TeduBlogConText CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            var builder = new DbContextOptionsBuilder<TeduBlogConText>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new TeduBlogConText(builder.Options);
        }
    }
}
