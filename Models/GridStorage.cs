using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverMaker.Models
{
    public class GridStorage: PointStorageBase
    {
        public LinkedPoint[,] Points { get; set; }

        public GridStorage(int pointsCount, int width, int height)
        {
            PointsCount = pointsCount;
            ImageWidth = width;
            ImageHeight = height;
        }
    }
}
