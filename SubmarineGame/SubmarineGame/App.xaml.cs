using Microsoft.Win32;
using SubmarineGame.Model;
using SubmarineGame.Persistence;
using SubmarineGame.View;
using SubmarineGame.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new SubmarineGameModel(new TextFilePersistence());
            _model.GameOver += new EventHandler<SubmarineEventArgs>(Model_GameOver);
            _model.TimePaused += new EventHandler<SubmarineEventArgs>(Model_TimePaused);
            _model.NewGame();

            _viewModel = new SubmarineGameViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new CancelEventHandler(View_Closing);
            _view.Show();

            _generatorTimer = new DispatcherTimer();
            _generatorTimer.Interval = TimeSpan.FromMilliseconds(3000);
            _generatorTimer.Tick += new EventHandler(GeneratorTimer_Tick);
            _generatorTimer.Start();
            _moverTimer = new DispatcherTimer();
            _moverTimer.Interval = TimeSpan.FromMilliseconds(100);
            _moverTimer.Tick += new EventHandler(MoverTimer_Tick);
            _moverTimer.Start();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void GeneratorTimer_Tick(object sender, EventArgs e)
        {
            _model.AddMine();

            if (_generatorTimer.Interval > TimeSpan.FromMilliseconds(1000))
            {
                _generatorTimer.Interval -= TimeSpan.FromMilliseconds(100);
            }
        }

        private void MoverTimer_Tick(object sender, EventArgs e)
        {
            _model.MoveMines();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.GameTimeElapse();
        }

        #endregion

        #region View event handlers

        private void View_Closing(object sender, CancelEventArgs e)
        {
            bool restartTimer = _timer.IsEnabled;

            StopTimers();

            if (MessageBox.Show("Are you sure you want to quit?", "Submarine Game", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;

                if (restartTimer)
                {
                    StartTimers();
                }
            }
        }

        #endregion

        #region ViewModel event handlers

        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            StopTimers();

            _model.NewGame();
            TurnOffPauseBackground();
            _generatorTimer.Interval = TimeSpan.FromMilliseconds(3000);
            StartTimers();
        }

        private void ViewModel_LoadGame(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Loading Submarine Game";
                openFileDialog.Filter = "Submarine Game file|*.smg";
                if (openFileDialog.ShowDialog() == true)
                {
                    _model.LoadGame(openFileDialog.FileName);
                }
            }
            catch (Persistence.DataException)
            {
                MessageBox.Show("Submarine Game file loading failed.", "Submarine Game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_SaveGame(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Saving Submarine Game";
                saveFileDialog.Filter = "Submarine Game file|*.smg";
                if (saveFileDialog.ShowDialog() == true)
                {
                    _model.SaveGame(saveFileDialog.FileName);
                }
            }
            catch (Persistence.DataException)
            {
                MessageBox.Show("Submarine Game file saving failed.", "Submarine Game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_ExitGame(object sender, EventArgs e)
        {
            _view.Close();
        }

        #endregion

        #region Model event handlers

        private void Model_GameOver(object sender, SubmarineEventArgs e)
        {
            StopTimers();
            _view.pauseBackground.Opacity = 0.5;
            MessageBox.Show("Your submarine exploded!" + Environment.NewLine +
                            "Time: " + TimeSpan.FromSeconds(e.GameTime).ToString("g") + Environment.NewLine +
                            "Destroyed mines: " + e.DestroyedMineCount.ToString("g"),
                            "Submarine Game",
                            MessageBoxButton.OK,
                            MessageBoxImage.Asterisk);

            _model.NewGame();
            _view.pauseBackground.Opacity = 0;
            _generatorTimer.Interval = TimeSpan.FromMilliseconds(3000);
            StartTimers();
        }

        private void Model_TimePaused(object sender, SubmarineEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                StopTimers();
                TurnOnPauseBackground();
            }
            else
            {
                StartTimers();
                TurnOffPauseBackground();
            }
        }

        #endregion

        #region Private methods

        private void StopTimers()
        {
            _timer.Stop();
            _moverTimer.Stop();
            _generatorTimer.Stop();
        }

        private void StartTimers()
        {
            _timer.Start();
            _moverTimer.Start();
            _generatorTimer.Start();
        }

        private void TurnOnPauseBackground()
        {
            _view.pauseBackground.Opacity = 0.5;
            _view.pause.Visibility = Visibility.Visible;
            _view.menu.IsEnabled = true;
        }

        private void TurnOffPauseBackground()
        {
            _view.pauseBackground.Opacity = 0;
            _view.pause.Visibility = Visibility.Hidden;
            _view.menu.IsEnabled = false;
        }

        #endregion
    }
}
