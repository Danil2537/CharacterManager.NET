using CharacterManager.Database;
using CharacterManager.Models;
using CharacterManager.Stores;
using CharacterManager.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CharacterManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //private const string CONNECTION_STRING = @"Data Source=C:\Documents\Labs\CharacterManager\charactermanager.db";
        public IServiceProvider ServiceProvider { get; private set; }
        private readonly NavigationStore _navigationStore;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            _navigationStore = new NavigationStore();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CharacterManagerDbContext>(options =>
                options.UseNpgsql("Server=localhost;Port=5432;Database=CharacterManager;User Id=postgres;Password=30072005"));
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            //using (var context = new CharacterManagerDbContext()) 
            //{
            //    context.Users.ToList();
            //}

            _navigationStore.CurrentViewModel = new LoginVM(new Services.NavigationService(_navigationStore, CreateProfileVM),
                      new Services.NavigationService(_navigationStore, CreateRegistrationVM));
            DbManager.GetAllClassesQuery();
            MainWindow = new MainWindow
            {
                DataContext = new MainVM(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
        private ViewModelBase CreateCreationVM(object parameter)
        {
            return new CreationVM(new Services.NavigationService(_navigationStore, CreateProfileVM), parameter as User);
        }
        private ViewModelBase CreateProfileVM(object parameter)
        {
            return new ProfileVM(new Services.NavigationService(_navigationStore, CreateCreationVM),
                new Services.NavigationService(_navigationStore, CreateSheetVM), new Services.NavigationService(_navigationStore, CreateCreateClassVM), parameter as User);

        }
        private ViewModelBase CreateLvlUpVM(object parameter)
        {
            return new LvlUpVM(new Services.NavigationService(_navigationStore, CreateSheetVM), parameter as Character);
        }
        private ViewModelBase CreateSheetVM(object parameter)
        {
            return new SheetVM(new Services.NavigationService(_navigationStore, CreateProfileVM), new Services.NavigationService(_navigationStore, CreateLvlUpVM), parameter as Character);
        }
        private ViewModelBase CreateLoginVM(object parameter)
        {
            return new LoginVM(new Services.NavigationService(_navigationStore, CreateProfileVM), new Services.NavigationService(_navigationStore, CreateRegistrationVM));
        }
        private ViewModelBase CreateRegistrationVM(object parameter)
        {
            return new RegistrationVM(new Services.NavigationService(_navigationStore, CreateLoginVM));
        }
        private ViewModelBase CreateCreateClassVM(object parameter)
        {
            return new CreateClassVM(new Services.NavigationService(_navigationStore, CreateProfileVM));
        }

    }
}
