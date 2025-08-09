using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdxDesigner.ViewModels;
using KdxDesigner.Services.MnemonicDevice;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

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

            // MainViewModelをシングルトンとして登録
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
