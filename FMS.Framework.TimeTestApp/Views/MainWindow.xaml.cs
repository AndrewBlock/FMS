using FMS.Framework.TimeTestApp.ViewModels;

namespace FMS.Framework.TimeTestApp.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
