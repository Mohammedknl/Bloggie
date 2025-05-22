using Bloggie.Web.Data; //BloggieDbContext class is from Data folder used in Servicces
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container.
builder.Services.AddControllersWithViews();

//Adding another service like injecting BloggieDbContext class in to our App services using Dependencies injection using options like
//we have to use 
//This is when our app talks to Db it should handle all the connections and Instances and provide the data smoothly.

//Here we say use SQL Server and ConnectionString same as appsettings.json ie we use Default here
builder.Services.AddDbContext<BloggieDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//With the above whenever we want to use DbContext we can use with the builder services from here 

//  OR
//options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));  for SQL SSMS Server with BloggieDbConnection string

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
