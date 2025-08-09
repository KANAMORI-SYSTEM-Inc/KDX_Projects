using Microsoft.EntityFrameworkCore;
using KdxMigrationAPI.Data;
using KdxMigrationAPI.Services;

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