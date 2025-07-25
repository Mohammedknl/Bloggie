using System.Text.Json;
using Bloggie.Web.Data; //BloggieDbContext class is from Data folder used in Servicces
using Bloggie.Web.Repositories;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container.
builder.Services.AddControllersWithViews();

//***New code added here for API Request

// Configure HTTP Client for OMRC API
builder.Services.AddHttpClient<OMRCApiService>(client =>
{
    var baseUrl = builder.Configuration.GetValue<string>("ApiSettings:OMRCApiBaseUrl") ?? "https://localhost:7036/";
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30); // Set timeout
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler()
    {
        // For development - ignore SSL certificate errors
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
});

// Register the service
builder.Services.AddScoped<IOMRCApiService, OMRCApiService>();

// Configure JSON serialization options globally
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Configure logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});




//New code ends here***

//Adding another service like injecting BloggieDbContext class in to our App services using Dependencies injection using options like
//we can use these services of BloggieDbContext whereever required like inside Controllers or Repositories
//This is when our app talks to Db it should handle all the connections and Instances and provide the data smoothly.

//Here we say use SQL Server and ConnectionString same as appsettings.json ie we use DefaultConnection here
builder.Services.AddDbContext<BloggieDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//With the above whenever we want to use DbContext we can use with the builder services from here 

//  OR
//options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));  for SQL SSMS Server with BloggieDbConnection string
//Injecting one more DbContext ie AuthDbContext services for Authorization and Authentication using builder object
builder.Services.AddDbContext<AuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));
//we have to inform that we r using Identity for AuthDbCOntext using Identity User and Identity Role classes
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});






//Adding new service Add a Injection inside services like when some one call the ItagRepository give back the implementation of TagRepository
builder.Services.AddScoped<ITagRepository,TagRepository>();
//Adding new service or injecting IBlogRepository for using
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
//Injecting ImageRepository for uploading images using cloudinaryImageRepository Implementation
builder.Services.AddScoped<IImageRespository, CloudinaryImageRepository>();
//Injecting IBlogPostLikeRepository
builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();
//Injecting IBlogPostCommentRepository
builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();
//Injecting IUserRepository service
builder.Services.AddScoped<IUserRepository, UserRepository>();


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

//Before Authorization we need to Authenticate the user first
app.UseAuthentication(); //New Middlewear provided by ASP.Net core

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
