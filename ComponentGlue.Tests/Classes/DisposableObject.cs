using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class DisposableObject : IDisposable
    {
        public bool WasDisposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            this.WasDisposed = true;
        }
    }
}
