using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.Concrete;
using BTKRoadmapperAPI.Data;
using BTKRoadmapperAPI.Entities;
using BTKRoadmapperAPI.Mapping;
using BTKRoadmapperAPI.Middlewares;
using BTKRoadmapperAPI.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables();


});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("SessionId");
    });
});
builder.Services.AddDbContext<BTKRoadmapperDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseSettings")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("CacheSettings:ConnectionString").Value;
});
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<HttpService>();
builder.Services.AddScoped<GeminiService>();
builder.Services.AddHttpClient<HttpService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<BTKRoadmapperDbContext>();

    var retryPolicy = Policy
        .Handle<NpgsqlException>()
        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(5), (exception, timeSpan, retryCount, context) =>
        {
            logger.LogWarning($"Baðlantý denemesi {retryCount} sýrasýnda hata: {exception.Message}. {timeSpan} saniye sonra tekrar deneniyor...");
        });

    try
    {
        await retryPolicy.ExecuteAsync(async () =>
        {
            await context.Database.MigrateAsync();
        });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Veritabaný baðlantýsý sýrasýnda bir hata oluþtu.");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
