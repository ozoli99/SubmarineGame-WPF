using System;

namespace SubmarineGame.Model
{
    public class SubmarineEventArgs : EventArgs
    {
        public int GameTime { get; private set; }
        public int DestroyedMineCount { get; private set; }
        public bool MoveUp { get; private set; }
        public bool MoveDown { get; private set; }
        public bool MoveLeft { get; private set; }
        public bool MoveRight { get; private set; }

        public SubmarineEventArgs(int gameTime, int destroyedMineCount, bool moveUp, bool moveDown, bool moveLeft, bool moveRight)
        {
            GameTime = gameTime;
            DestroyedMineCount = destroyedMineCount;
            MoveUp = moveUp;
            MoveDown = moveDown;
            MoveLeft = moveLeft;
            MoveRight = moveRight;
        }
    }
}
