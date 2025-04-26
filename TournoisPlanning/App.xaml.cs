using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using TournoisPlanning.Config;
using TournoisPlanning.Services;
using TournoisPlanning.ViewModels;

namespace TournoisPlanning;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    // In App.xaml.cs or a bootstrapper class
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Register your services
        ServiceLocator.RegisterService<ITournoiService>(new TournoiService());
        ServiceLocator.RegisterService<IEquipeService>(new EquipeService());
        ServiceLocator.RegisterService<INavigationService>(new TournoisPlanning.Services.NavigationService());
        //// Create database connection
        var dbConnection = new DBConn("127.0.0.1", "adminTM", "firstproject", "tournoisManagement");

        //// Register services
        ServiceLocator.RegisterService<ITournoiService>(new DbTournoiService(dbConnection));
        //ServiceLocator.RegisterService<Dashboard>(new Dashboard(dbConnection));
        ServiceLocator.RegisterService<INavigationService>(new TournoisPlanning.Services.NavigationService());
        // Continue with application startup...
    }
}

