using Microsoft.EntityFrameworkCore;
using KdxMigrationAPI.Data;
using KdxMigrationAPI.Services;
using Kdx.Core.Application;
using Kdx.Core.Domain.Services;
using Kdx.Infrastructure.Cache;
using Kdx.Infrastructure.Options;
using Kdx.Infrastructure.Repositories;
using Kdx.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// NSwag設定
builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Title = "KDX Migration API";
        document.Info.Version = "v1";
        document.Info.Description = "API for KDX system data migration and management";
    };
});

// PostgreSQL DbContext設定
builder.Services.AddDbContext<MigrationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// サービスの登録
builder.Services.AddScoped<AccessDataService>();
builder.Services.AddScoped<MigrationService>();

// 5層アーキテクチャのサービス登録
// Options
builder.Services.Configure<DeviceOffsetOptions>(options =>
{
    options.DeviceStartT = builder.Configuration.GetValue<int>("DeviceOffsets:DeviceStartT", 0);
    options.TimerStartZR = builder.Configuration.GetValue<int>("DeviceOffsets:TimerStartZR", 0);
});

// Domain Services
builder.Services.AddSingleton<IDeviceOffsetProvider, DeviceOffsetProvider>();
builder.Services.AddSingleton<ISequenceGenerator, SequenceGenerator>();

// Infrastructure
builder.Services.AddSingleton<Kdx.Core.Domain.Interfaces.IMnemonicDeviceMemoryStore, Kdx.Infrastructure.Cache.MnemonicDeviceMemoryStore>();
builder.Services.AddSingleton<ITimerDeviceCache, TimerDeviceCache>();
builder.Services.AddScoped<Kdx.Core.Domain.Interfaces.IAccessRepository>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("AccessConnection") 
        ?? throw new InvalidOperationException("AccessConnection string not configured");
    return new Kdx.Infrastructure.Adapters.AccessRepositoryAdapter(connectionString);
});
builder.Services.AddScoped<IMnemonicTimerDeviceRepository, MnemonicTimerDeviceRepository>();

// Application Use Cases
builder.Services.AddScoped<SaveProcessDetailTimerDevicesUseCase>();

// API Services
builder.Services.AddScoped<ITimerDeviceService, TimerDeviceService>();

// CORS設定（開発環境用）
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // NSwag設定
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseCors("DevelopmentPolicy");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// データベースマイグレーション自動実行（開発環境のみ）
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MigrationDbContext>();
        try
        {
            context.Database.Migrate();
            Console.WriteLine("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database migration failed: {ex.Message}");
        }
    }
}

app.Run();