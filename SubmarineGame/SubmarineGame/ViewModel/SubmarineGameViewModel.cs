using SubmarineGame.Model;
using System;
using System.Collections.ObjectModel;

namespace SubmarineGame.ViewModel
{
    public class SubmarineGameViewModel : ViewModelBase
    {
        #region Fields

        private SubmarineGameModel _model;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<Shape> Mines { get; set; }
        public Submarine Submarine { get; set; }

        public int DestroyedMineCount { get { return _model.DestroyedMineCount; } }
        public string GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }

        #endregion

        #region Events



        #endregion
    }
}
