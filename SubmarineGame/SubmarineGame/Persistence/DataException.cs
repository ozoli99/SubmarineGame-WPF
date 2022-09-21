using System;

namespace SubmarineGame.Persistence
{
    public class DataException : Exception
    {
        public DataException(String message) : base(message) { }
    }
}
