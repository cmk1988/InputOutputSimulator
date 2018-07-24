using InputOutputSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BusinessLogic bl = new BusinessLogic
                (
                100,
                100,
                "",
                "",
                "",
                "",
                "",
                120
                );
            bl.UndLos();
        }
    }
}
