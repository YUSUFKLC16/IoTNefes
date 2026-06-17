using System.IO.Compression;
using System.Threading.RateLimiting;
using IotNefes.Application.Example.Mapping;
using IotNefes.Infastructure.Extensions;
using IotNefes.Persistance;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IotNefes.WebApi.src.Filters.ETagFilter>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Response Compression - Gzip (primary) + Brotli (fallback)
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    options.Level = CompressionLevel.Fastest);
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    options.Level = CompressionLevel.Fastest);

// Rate Limiting - Fixed Window (configurable from appsettings.json)
var rateLimitConfig = builder.Configuration.GetSection("RateLimiting");
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = rateLimitConfig.GetValue<int>("PermitLimit", 100);
        opt.Window = TimeSpan.FromSeconds(rateLimitConfig.GetValue<int>("WindowInSeconds", 60));
        opt.QueueLimit = rateLimitConfig.GetValue<int>("QueueLimit", 0);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Çok fazla istek gönderildi. Lütfen biraz bekleyin.",
            retryAfterSeconds = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                ? (int)retryAfter.TotalSeconds
                : 60
        }, cancellationToken);
    };
});

// Output Caching (configurable from appsettings.json)
var cacheDuration = builder.Configuration.GetValue<int>("Caching:DefaultDurationSeconds", 30);
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(policy => policy.NoCache());
    options.AddPolicy("CacheGet", policy =>
        policy.Expire(TimeSpan.FromSeconds(cacheDuration))
              .SetVaryByQuery(["page", "pageSize"])
              .Tag("all"));
});

// CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["http://localhost:3000"])
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// Persistance (MongoDB + Repositories)
builder.Services.AddPersistance(builder.Configuration);

// Auto-register services by marker interfaces (IScopedService, ITransientService, ISingletonService)
builder.Services.AddServicesByConvention(
    typeof(ExampleEntityMapper).Assembly,
    typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();

app.UseCors("AllowReactApp");

app.UseOutputCache();

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireRateLimiting("fixed");

app.Run();