﻿using SubmarineGame.Model;
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
    }
}
