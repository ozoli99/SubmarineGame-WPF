namespace SubmarineGame.ViewModel
{
    public class Shape : ViewModelBase
    {
        private int _x;
        private int _y;

        public int X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
    }
}
