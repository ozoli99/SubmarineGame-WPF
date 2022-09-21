using System;
using System.Collections.Generic;
using System.IO;

namespace SubmarineGame.Persistence
{
    public class TextFilePersistence : IPersistence
    {
        public List<Shape> Load(string path, ref int gameTime, ref int destroyedMineCount)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string[] numbers = reader.ReadToEnd().Split();

                    gameTime = int.Parse(numbers[0]);
                    destroyedMineCount = int.Parse(numbers[1]);

                    List<Shape> submarineAndMines = new List<Shape>();

                    ShapeType type = (ShapeType)int.Parse(numbers[3]);
                    int x = int.Parse(numbers[4]);
                    int y = int.Parse(numbers[5]);
                    int width = int.Parse(numbers[6]);
                    int height = int.Parse(numbers[7]);
                    int weight = int.Parse(numbers[8]);
                    Shape submarine = new Shape(type, x, y, width, height, weight);
                    submarineAndMines.Add(submarine);

                    for (int i = 10; i < numbers.Length - 3; i += 7)
                    {
                        type = (ShapeType)int.Parse(numbers[i]);
                        x = int.Parse(numbers[i + 1]);
                        y = int.Parse(numbers[i + 2]);
                        width = int.Parse(numbers[i + 3]);
                        height = int.Parse(numbers[i + 4]);
                        weight = int.Parse(numbers[i + 5]);
                        submarineAndMines.Add(new Shape(type, x, y, weight, height, weight));
                    }
                    return submarineAndMines;
                }
            }
            catch
            {
                throw new DataException("Error occurred during reading.");
            }
        }

        public void Save(string path, List<Shape> mines, Shape submarine, int gameTime, int destroyedMineCount)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (mines == null)
                throw new ArgumentNullException("mines");
            if (submarine == null)
                throw new ArgumentNullException("submarine");

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine((int)gameTime + " " + (int)destroyedMineCount);
                    writer.WriteLine((int)submarine.Type + " " + (int)submarine.X + " " + (int)submarine.Y + " " + (int)submarine.Width + " " + (int)submarine.Height + " " + (int)submarine.Weight);
                    for (int i = 0; i < mines.Count; ++i)
                    {
                        writer.WriteLine((int)mines[i].Type + " " + (int)mines[i].X + " " + (int)mines[i].Y + " " + (int)mines[i].Width + " " + (int)mines[i].Height + " " + (int)mines[i].Weight);
                    }
                }
            }
            catch
            {
                throw new DataException("Error occurred during writing.");
            }
        }
    }
}