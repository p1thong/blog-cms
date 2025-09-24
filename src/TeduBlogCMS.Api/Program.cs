using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TeduBlogCMS.Api;
using TeduBlogCMS.Core.Domain.Identity;
using TeduBlogCMS.Core.Models.PostDto;
using TeduBlogCMS.Core.Repositories.Posts;
using TeduBlogCMS.Core.SeedWorks;
using TeduBlogCMS.Data;
using TeduBlogCMS.Data.Repositories.Posts;
using TeduBlogCMS.Data.SeedWorks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TeduBlogConText>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder
    .Services.AddIdentity<AppUser, AppRole>(options =>
        options.SignIn.RequireConfirmedAccount = false
    )
    .AddEntityFrameworkStores<TeduBlogConText>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password setting
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    //Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

//Add services to the container
builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Business services and repositories
var services = typeof(PostRepository)
    .Assembly.GetTypes()
    .Where(x =>
        x.GetInterfaces().Any(i => i.Name == typeof(IRepository<,>).Name)
        && !x.IsAbstract
        && x.IsClass
        && !x.IsGenericType
    );

foreach (var service in services)
{
    var allInterfaces = service.GetInterfaces();
    var directInterface = allInterfaces
        .Except(allInterfaces.SelectMany(t => t.GetInterfaces()))
        .FirstOrDefault();
    if (directInterface != null)
    {
        builder.Services.Add(
            new ServiceDescriptor(directInterface, service, ServiceLifetime.Scoped)
        );
    }
}

//add automapper
builder.Services.AddAutoMapper(typeof(PostInListDto));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Seeding data
app.MigrateDatabase();

app.Run();
