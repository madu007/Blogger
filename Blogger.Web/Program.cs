using Blogger.Web.Data;
using Blogger.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var services = builder.Services;

services.AddScoped<ITagRepository, TagRepository>();
services.AddScoped<IBlogPostRepository, BlogPostRepository>();
services.AddScoped<IImageRepository, ImageRepository>();
services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();
services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();
services.AddScoped<IUserRepository, UserRepository>();

services.AddDbContext<BloggerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggerConnection")));

services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggerAuthConnection")));

services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

services.Configure<IdentityOptions>(option =>
{
    option.Password.RequiredUniqueChars = 1;
    option.Password.RequireUppercase = true;
    option.Password.RequireLowercase = true;
    option.Password.RequireDigit = true;
    option.Password.RequireNonAlphanumeric = true;
    option.Password.RequiredLength = 6;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
