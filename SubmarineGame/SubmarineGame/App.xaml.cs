using SubmarineGame.Model;
using SubmarineGame.View;
using SubmarineGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SubmarineGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private SubmarineGameModel _model;
        private SubmarineGameViewModel _viewModel;
        private MainWindow _view;
        private DispatcherTimer _generatorTimer;
        private DispatcherTimer _moverTimer;
        private DispatcherTimer _timer;

        #endregion

        #region Constructor

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion
    }
}
