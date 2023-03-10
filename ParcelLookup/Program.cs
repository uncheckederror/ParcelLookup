using ParcelLookup.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
    builder.Expire(TimeSpan.FromSeconds(30)));
});
AppConfiguration config = new();
builder.Configuration.Bind(config);
builder.Services.AddSingleton(config);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSecurityHeaders();

app.UseHttpsRedirection();
// https://stackoverflow.com/questions/68484201/static-file-caching-in-asp-net-core
var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
             "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
    }
});

app.UseRouting();
app.UseOutputCache();

app.UseAuthorization();

app.MapRazorPages().CacheOutput();

app.Run();
