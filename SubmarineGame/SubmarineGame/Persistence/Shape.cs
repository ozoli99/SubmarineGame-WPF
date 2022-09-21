namespace SubmarineGame.Persistence
{
    public enum ShapeType { Submarine, Mine }
    public class Shape
    {
        public ShapeType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Weight { get; set; }

        public Shape(ShapeType type, int startX, int startY, int width, int height, int weight)
        {
            Type = type;
            X = startX;
            Y = startY;
            Width = width;
            Height = height;
            Weight = weight;
        }
    }
}
