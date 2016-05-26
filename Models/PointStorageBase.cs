using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverMaker.Models
{
    public class PointStorageBase
    {
        public static double WATER_INCOME_ADDITION_COEFFICIENT = 0.75;
        public int pointsCount = 0;
        public LinkedPoint[] points;
        public bool isLinked = false;

        public PointStorageBase(int pointsCount_)
        {
            pointsCount = pointsCount_;
            points = new LinkedPoint[pointsCount];
        }

        public void DefineWaterIncome()
        {
            if(!isLinked)
            {
                throw new ArgumentException("points weren't linked");
            }
            else
            {
                for(int i = 0; i < pointsCount; ++i)
                {
                    if(!points[i].waterIncomeDefined)
                    {
                        DefineWaterIncomeForPoint(points[i]);
                    }
                }
            }
        }

        private double DefineWaterIncomeForPoint(LinkedPoint p)
        {
            if(p.waterIncomeDefined)
            {
                return p.ThisPoint.WaterIncome;
            }
            else
            {
                double sum = 0;
                for(int i = 0; i < p.Input.Count; ++i)
                {
                    sum += DefineWaterIncomeForPoint(p.Input[i]);
                }
                sum *= WATER_INCOME_ADDITION_COEFFICIENT;
                p.ThisPoint.WaterIncome += sum;
                p.waterIncomeDefined = true;
                return sum;
            }
        }

        public class LinkedPoint
        {
            public Point ThisPoint { get; set; }
            public List<LinkedPoint> Input { get; set; }
            public LinkedPoint Output { get; set; }
            public bool waterIncomeDefined = false;

            public LinkedPoint()
            {
                ThisPoint = new Point();
                Input = new List<LinkedPoint>();
                Output = null;
            }

            public LinkedPoint(double x, double y, double height)
            {
                ThisPoint = new Point(x, y, height);
                Input = new List<LinkedPoint>();
                Output = null;
            }
        }
    }
}
