using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RiverMaker.Controllers
{
    public class HeightDefiner
    {
        public static double GetWhiteHeight(Color color)
        {
            return (255-color.B) / ((double)255);
        }

        public static double GetBlackHeight(Color color)
        {
            return color.B / ((double)255);
        }
    }
}
