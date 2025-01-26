using MiniCore.Views;

namespace MiniCore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new EmpleadoPage());
        }
    }
}
