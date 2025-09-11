using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdxDesigner.ViewModels;
using KdxDesigner.Services.MnemonicDevice;
using KdxDesigner.Services.Authentication;
using KdxDesigner.Services;
using Kdx.Contracts.Interfaces;
using Kdx.Infrastructure.Repositories;
using Kdx.Infrastructure.Adapters;
using Kdx.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.IO;
using Supabase;

namespace KdxDesigner
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }

        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();
            
            services.AddSingleton<IConfiguration>(configuration);
            
            // Supabase設定を登録
            var supabaseConfig = configuration.GetSection("Supabase").Get<SupabaseConfiguration>();
            if (supabaseConfig != null)
            {
                System.Diagnostics.Debug.WriteLine($"Supabase URL: {supabaseConfig.Url}");
                System.Diagnostics.Debug.WriteLine($"Supabase AnonKey: {supabaseConfig.AnonKey?.Substring(0, 20)}...");
                services.AddSingleton(supabaseConfig);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("WARNING: Supabase configuration not found!");
            }
            
            // Supabaseクライアントを登録（初期化は遅延実行）
            services.AddSingleton<Supabase.Client>(sp =>
            {
                var config = sp.GetRequiredService<SupabaseConfiguration>();
                System.Diagnostics.Debug.WriteLine($"Creating Supabase Client with URL: {config.Url}");
                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = false, // 自動接続を無効化して手動制御
                    AutoRefreshToken = true
                };
                var client = new Supabase.Client(config.Url, config.AnonKey, options);
                
                // 初期化はここでは行わない（遅延実行）
                System.Diagnostics.Debug.WriteLine("Supabase Client created (not initialized yet)");
                
                return client;
            });
            
            // Repository層の登録
            services.AddScoped<ISupabaseRepository, SupabaseRepository>();
            services.AddScoped<IAccessRepository, SupabaseRepositoryAdapter>();
            
            // Service層の登録（Infrastructure）
            services.AddScoped<IProsTimeDeviceService, Kdx.Infrastructure.Services.ProsTimeDeviceService>();
            services.AddScoped<IMemoryService, Kdx.Infrastructure.Services.MemoryService>();
            
            // Supabase接続ヘルパーの登録
            services.AddSingleton<SupabaseConnectionHelper>();
            
            // 認証サービスの登録
            services.AddSingleton<ISessionStorageService, SessionStorageService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IOAuthCallbackListener, OAuthCallbackListener>();
            
            // ViewModelの登録
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            
            // MnemonicDeviceMemoryStoreをシングルトンとして登録
            // アプリケーション全体で共有されるメモリストア
            services.AddSingleton<IMnemonicDeviceMemoryStore, MnemonicDeviceMemoryStore>();

            // 他のServiceなどもここに登録できます
            // services.AddTransient<IMyService, MyService>();

            return services.BuildServiceProvider();
        }
    }
}
