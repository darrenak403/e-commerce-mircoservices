using ApiGateway.Presentation.Middleware;
using ECommerce.ShareLibrary.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
JWTAuthenticationScheme.AddJWTAuthenticationScheme(builder.Services, builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Thứ tự middleware rất quan trọng
app.UseHttpsRedirection();
app.UseCors();
app.UseMiddleware<AttachSignatureToRequest>();
app.UseAuthentication(); // Thêm dòng này
app.UseAuthorization();  // Thêm dòng này
await app.UseOcelot();

app.Run();
