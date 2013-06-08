using System;

namespace ComponentGlue
{
    public class AutoBindException : Exception
    {
        public AutoBindException(string message)
            : base(message)
        {
        }
    }
}
