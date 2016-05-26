using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverMaker.Models
{
    //All properties(besides WaterIncome) in this class are abstract: they can have value in range [0, 1].
    //Real values are defined in controller
    public class Point
    {
        public static double DEFAULT_WATER_INCOME = 0.5;
        private double x, y, height;
        public double X
        {
            get { return x; }
            set
            {
                if (value <= 1)
                {
                    if (value >= 0)
                        x = value;
                    else
                        x = 0;
                }
                else
                    x = 1;
            }
        }
        public double Y
        {
            get { return y; }
            set
            {
                if (value <= 1)
                {
                    if (value >= 0)
                        y = value;
                    else
                        y = 0;
                }
                else
                    y = 1;
            }
        }
        public double Height
        {
            get { return height; }
            set
            {
                if (value <= 1)
                {
                    if (value >= 0)
                        height = value;
                    else
                        height = 0;
                }
                else
                    height = 1;
            }
        }

        public double WaterIncome { get; set; }

        public Point()
        {
            WaterIncome = DEFAULT_WATER_INCOME;
            X = 0.0;
            Y = 0.0;
            Height = 0.0;
        }

        public Point(double x, double y, double height)
        {
            WaterIncome = DEFAULT_WATER_INCOME;
            X = x;
            Y = y;
            Height = height;
        }
    }
}
