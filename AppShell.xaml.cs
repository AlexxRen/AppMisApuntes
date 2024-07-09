namespace Apuntes
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.ARNotePage), typeof(Views.ARNotePage));
        }
    }
}
