using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class Has2Params
    {
        public string Name
        {
            get;
            private set;
        }

        public int Age
        {
            get;
            private set;
        }

        public Has2Params(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
    }
}
