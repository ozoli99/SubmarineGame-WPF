using SubmarineGame.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SubmarineGame.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SubmarineGameViewModel ViewModel { get { return DataContext as SubmarineGameViewModel; } }
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (ViewModel == null)
                return;

            ViewModel.SubmarineStep(e.Key.ToString());

            ImageBrush submarineImage = new ImageBrush();

            switch (e.Key)
            {
                case Key.Left:
                case Key.A:
                    submarineImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/submarineLeft.png"));
                    submarine.Source = submarineImage.ImageSource;
                    break;
                case Key.Right:
                case Key.D:
                    submarineImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/submarine.png"));
                    submarine.Source = submarineImage.ImageSource;
                    break;
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            submarine.Focus();
        }
    }
}
