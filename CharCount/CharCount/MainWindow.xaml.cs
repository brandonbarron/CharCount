using MahApps.Metro.Controls;

namespace CharCount {
    public partial class MainWindow : MetroWindow {
        public MainWindow() {
            InitializeComponent();
            DataContext = new MainModelView();
        }

    }
}
