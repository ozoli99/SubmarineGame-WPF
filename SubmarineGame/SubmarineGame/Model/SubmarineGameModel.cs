using Persistence;
using System;
using System.Collections.Generic;

namespace Model
{
    public class SubmarineGameModel
    {
        #region Constants

        public const int GameAreaWidth = 940;
        public const int GameAreaHeight = 700;
        public const int SubmarineSize = 64;
        public const int MineSize = 64;
        public const int SubmarineStep = 10;
        public const int MineStep = 5;

        #endregion

        #region Private fields

        private Shape _submarine;
        private List<Shape> _mines;
        private Random _random;
        private int _destroyedMineCount;
        private int _gameTime;
        private IPersistence _persistence;

        #endregion

        #region Events

        public event EventHandler<SubmarineEventArgs> GameOver;
        public event EventHandler<SubmarineEventArgs> SubmarineMoved;
        public event EventHandler<SubmarineEventArgs> TimePaused;
        public event EventHandler<MineEventArgs> MineDestroyed;
        public event EventHandler<MineEventArgs> MineMoved;
        public event EventHandler<MineEventArgs> MineAdded;
        public event EventHandler GameCreated;
        public event EventHandler GameTimeElapsed;

        #endregion

        #region Properties

        public Shape Submarine { get { return _submarine; } }
        public IList<Shape> Mines { get { return _mines.AsReadOnly(); } }
        public int DestroyedMineCount { get { return _destroyedMineCount; } }
        public int GameTime { get { return _gameTime; } }
        public bool IsGameOver { get; set; }

        #endregion

        #region Constructors

        public SubmarineGameModel() : this(null) { }
        public SubmarineGameModel(IPersistence persistence)
        {
            _destroyedMineCount = 0;
            _gameTime = 0;
            IsGameOver = false;

            _random = new Random();

            _submarine = new Shape(ShapeType.Submarine, (GameAreaWidth - SubmarineSize) / 2, GameAreaHeight - SubmarineSize, SubmarineSize, SubmarineSize, 1);
            _mines = new List<Shape>();

            _persistence = persistence;
        }

        #endregion

        #region Public methods

        public void NewGame()
        {
            _destroyedMineCount = 0;
            _gameTime = 0;

            _submarine.X = (GameAreaWidth - SubmarineSize) / 2;
            _submarine.Y = (GameAreaHeight - SubmarineSize);

            _mines.Clear();
            GenerateStartingMines();

            GameCreated?.Invoke(this, EventArgs.Empty);
        }

        public void LoadGame(String fileName)
        {
            if (_persistence == null)
                return;

            List<Shape> submarineAndMines = _persistence.Load(fileName, ref _gameTime, ref _destroyedMineCount);

            _submarine = submarineAndMines[0];
            submarineAndMines.RemoveAt(0);

            _mines.Clear();
            _mines = submarineAndMines;

            GameCreated?.Invoke(this, EventArgs.Empty);

            CheckGame();
        }

        public void SaveGame(String fileName)
        {
            if (_persistence == null)
                return;

            List<Shape> mines = new List<Shape>();
            for (int i = 0; i < _mines.Count; ++i)
            {
                mines.Add(_mines[i]);
            }
            Shape submarine = _submarine;
            int gameTime = _gameTime;
            int destroyedMineCount = _destroyedMineCount;

            _persistence.Save(fileName, mines, submarine, gameTime, destroyedMineCount);
        }

        public void GameTimeElapse()
        {
            if (IsGameOver)
                return;

            _gameTime++;
            GameTimeElapsed?.Invoke(this, EventArgs.Empty);
        }

        public void PauseGame()
        {
            TimePaused?.Invoke(this, new SubmarineEventArgs(_gameTime, _destroyedMineCount, false, false, false, false));
        }

        public void Submarine_MoveUp()
        {
            if (_submarine.Y > SubmarineStep + 24)
            {
                _submarine.Y = _submarine.Y - SubmarineStep;

                SubmarineMoved?.Invoke(this, new SubmarineEventArgs(_gameTime, _destroyedMineCount, true, false, false, false));
                CheckGame();
            }
        }

        public void Submarine_MoveDown()
        {
            if (_submarine.Y < (GameAreaHeight - _submarine.Height) - SubmarineStep)
            {
                _submarine.Y = _submarine.Y + SubmarineStep;
                SubmarineMoved?.Invoke(this, new SubmarineEventArgs(_gameTime, _destroyedMineCount, false, true, false, false));
                CheckGame();
            }
        }

        public void Submarine_MoveLeft()
        {
            if (_submarine.X > SubmarineStep)
            {
                _submarine.X = _submarine.X - SubmarineStep;
                SubmarineMoved?.Invoke(this, new SubmarineEventArgs(_gameTime, _destroyedMineCount, false, false, true, false));
                CheckGame();
            }
        }

        public void Submarine_MoveRight()
        {
            if (_submarine.X < (GameAreaWidth - _submarine.Width) - SubmarineStep)
            {
                _submarine.X = _submarine.X + SubmarineStep;
                SubmarineMoved?.Invoke(this, new SubmarineEventArgs(_gameTime, _destroyedMineCount, false, false, false, true));
                CheckGame();
            }
        }

        public void MoveMines()
        {
            for (int i = 0; i < _mines.Count; ++i)
            {
                _mines[i].Y = _mines[i].Y + (_mines[i].Weight * MineStep);
                MineMoved?.Invoke(this, new MineEventArgs(i, _destroyedMineCount));
                if (_mines[i].Y > GameAreaHeight)
                {
                    _mines.RemoveAt(i);
                    _destroyedMineCount++;
                    MineDestroyed?.Invoke(this, new MineEventArgs(i, _destroyedMineCount));
                    i--;
                }
            }

            CheckGame();
        }

        public void AddMine()
        {
            int mineX = _random.Next(1, GameAreaWidth - MineSize);
            int mineWeight = _random.Next(1, 4);

            Shape newMine = new Shape(ShapeType.Mine, mineX, 0, MineSize, MineSize, mineWeight);

            while (_mines.Contains(newMine))
            {
                mineX = _random.Next(1, GameAreaWidth - MineSize);
                mineWeight = _random.Next(1, 4);

                newMine = new Shape(ShapeType.Mine, mineX, 0, MineSize, MineSize, mineWeight);
            }

            _mines.Add(newMine);
            MineAdded?.Invoke(this, new MineEventArgs(_mines.Count - 1, _destroyedMineCount));
        }

        #endregion

        #region Private methods

        private void GenerateStartingMines()
        {
            for (int i = 0; i < 6; ++i)
            {
                AddMine();
            }
        }

        private void CheckGame()
        {
            for (int i = 0; i < _mines.Count; ++i)
            {
                bool collisionLeftAndBottom = (_mines[i].X <= _submarine.X && (_mines[i].X + _mines[i].Width) >= _submarine.X) && (_mines[i].Y <= _submarine.Y && (_mines[i].Y + _mines[i].Height) >= _submarine.Y);
                bool collisionRightAndBottom = (_mines[i].X <= (_submarine.X + _submarine.Width) && (_mines[i].X + _mines[i].Width) >= (_submarine.X + _submarine.Width)) && (_mines[i].Y <= _submarine.Y && (_mines[i].Y + _mines[i].Height) >= _submarine.Y);
                bool collisionLeftAndTop = (_mines[i].X <= _submarine.X && (_mines[i].X + _mines[i].Width) >= _submarine.X) && (_mines[i].Y <= (_submarine.Y + _submarine.Height) && (_mines[i].Y + _mines[i].Height) >= (_submarine.Y + _submarine.Height));
                bool collisionRightAndTop = (_mines[i].X <= (_submarine.X + _submarine.Width) && (_mines[i].X + _mines[i].Width) >= (_submarine.X + _submarine.Width) && (_mines[i].Y <= (_submarine.Y + _submarine.Height)) && (_mines[i].Y + _mines[i].Height) >= (_submarine.Y + _submarine.Height));

                if (collisionLeftAndBottom || collisionRightAndBottom || collisionLeftAndTop || collisionRightAndTop)
                {
                    _gameTime = 0;
                    _destroyedMineCount = 0;
                    GameOver?.Invoke(this, new SubmarineEventArgs(_gameTime, _destroyedMineCount, false, false, false, false));
                }
            }
        }

        #endregion
    }
}