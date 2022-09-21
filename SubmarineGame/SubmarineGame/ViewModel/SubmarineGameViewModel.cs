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

        public event EventHandler NewGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        public event EventHandler ExitGame;

        #endregion

        #region Constructor

        public SubmarineGameViewModel(SubmarineGameModel model)
        {
            _model = model;
            _model.SubmarineMoved += new EventHandler<SubmarineEventArgs>(Model_SubmarineMoved);
            _model.MineMoved += new EventHandler<MineEventArgs>(Model_MineMoved);
            _model.MineDestroyed += new EventHandler<MineEventArgs>(Model_MineDestroyed);
            _model.MineAdded += new EventHandler<MineEventArgs>(Model_MineAdded);
            _model.GameCreated += new EventHandler(Model_GameCreated);
            _model.GameTimeElapsed += new EventHandler(Model_GameTimeElapsed);

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            Mines = new ObservableCollection<Shape>();
            for (int i = 0; i < _model.Mines.Count; i++)
            {
                Mines.Add(new Shape
                {
                    X = _model.Mines[i].X,
                    Y = _model.Mines[i].Y,
                    Width = _model.Mines[i].Width,
                    Height = _model.Mines[i].Height,
                    Weight = _model.Mines[i].Weight,
                });
            }
            Submarine = new Submarine
            {
                X = _model.Submarine.X,
                Y = _model.Submarine.Y,
                Width = _model.Submarine.Width,
                Height = _model.Submarine.Height,
                Weight = _model.Submarine.Weight,
                StepCommand = new DelegateCommand(param => SubmarineStep(param.ToString()))
            };
        }

        #endregion

        #region Public methods

        public void SubmarineStep(string stepDirection)
        {
            switch (stepDirection)
            {
                case "Up": case "W": _model.Submarine_MoveUp(); break;
                case "Down": case "S": _model.Submarine_MoveDown(); break;
                case "Left": case "A": _model.Submarine_MoveLeft(); break;
                case "Right": case "D": _model.Submarine_MoveRight(); break;
                case "Space": _model.PauseGame(); break;
            }
        }

        #endregion

        #region Model event handlers

        private void Model_SubmarineMoved(object sender, SubmarineEventArgs e)
        {
            Submarine.X = _model.Submarine.X;
            Submarine.Y = _model.Submarine.Y;
        }

        private void Model_MineMoved(object sender, MineEventArgs e)
        {
            Mines[e.MineID].Y = _model.Mines[e.MineID].Y;
        }

        private void Model_MineDestroyed(object sender, MineEventArgs e)
        {
            Mines.RemoveAt(e.MineID);
            OnPropertyChanged("DestroyedMineCount");
        }

        private void Model_MineAdded(object sender, MineEventArgs e)
        {
            if (_model.Mines.Count > Mines.Count)
            {
                Mines.Add(new Shape
                {
                    X = _model.Mines[e.MineID].X,
                    Y = _model.Mines[e.MineID].Y,
                    Width = _model.Mines[e.MineID].Width,
                    Height = _model.Mines[e.MineID].Height,
                    Weight = _model.Mines[e.MineID].Weight
                });
            }
        }

        private void Model_GameCreated(object sender, EventArgs e)
        {
            Mines.Clear();
            for (int i = 0; i < _model.Mines.Count; i++)
            {
                Mines.Add(new Shape
                {
                    X = _model.Mines[i].X,
                    Y = _model.Mines[i].Y,
                    Width = _model.Mines[i].Width,
                    Height = _model.Mines[i].Height,
                    Weight = _model.Mines[i].Weight
                });
            }
            Submarine.X = _model.Submarine.X;
            Submarine.Y = _model.Submarine.Y;
            Submarine.Width = _model.Submarine.Width;
            Submarine.Height = _model.Submarine.Height;
            Submarine.Weight = _model.Submarine.Weight;
        }

        private void Model_GameTimeElapsed(object sender, EventArgs e)
        {
            OnPropertyChanged("GameTime");
        }

        #endregion

        #region Event methods

        private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }

        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        #endregion
    }
}
