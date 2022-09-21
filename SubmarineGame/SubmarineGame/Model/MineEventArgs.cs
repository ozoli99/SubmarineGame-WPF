using System;

namespace SubmarineGame.Model
{
    public class MineEventArgs : EventArgs
    {
        public int MineID { get; private set; }
        public int DestroyedMineCount { get; private set; }

        public MineEventArgs(int mineId, int destroyedMineCount)
        {
            MineID = mineId;
            DestroyedMineCount = destroyedMineCount;
        }
    }
}