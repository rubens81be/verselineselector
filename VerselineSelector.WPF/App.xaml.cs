using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using VerselineSelector.DAL.Core;
using VerselineSelector.DAL.Persistence;
using VerselineSelector.DAL.Repositories;
using VerselineSelector.WPF.ViewModel;
using VerselineSelector.WPF.Services;
using VerselineSelector.WPF.View;

namespace VerselineSelector.WPF;
public partial class App : Application
{
    [STAThread]
    public static void Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        host.Start();

        using (var scope = host.Services.CreateScope())
        using (var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
        {
            dbContext.Database.EnsureCreated();
        };

        App app = new()
        {
            MainWindow = host.Services.GetRequiredService<MainWindow>()
        };

        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => 
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) => 
            {
                configurationBuilder.AddJsonFile("appsettings.json", false, true).Build();
            })
            .ConfigureServices((hostBuilderContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddTransient<IVerselineRepository, VerselineRepository>();
                services.AddTransient<IParagraphRepository, ParagraphRepository>();
                services.AddTransient<IPatientRepository, PatientRepository>();
                services.AddTransient<IUnitOfWork, UnitOfWork>();

                services.AddSingleton<IHighlightingService, TextHighlightingService>();


                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(hostBuilderContext.Configuration.GetConnectionString("SamDatabase"));                    
                    options.UseLazyLoadingProxies(useLazyLoadingProxies: true);
                });                
            });
}
