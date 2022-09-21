using System.Collections.Generic;

namespace SubmarineGame.Persistence
{
    public interface IPersistence
    {
        List<Shape> Load(string path, ref int gameTime, ref int destroyedMineCount);
        void Save(string path, List<Shape> mines, Shape submarine, int gameTime, int destroyedMineCount);
    }
}