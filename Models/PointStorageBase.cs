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
        public static double WATER_INCOME_ADDITION_FOR_SAME_LVL_COEFFICIENT = 0.5;
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
                //defining water income for points on different levels
                for(int i = 0; i < pointsCount; ++i)
                {
                    if(!points[i].waterIncomeDefined)
                    {
                        DefineWaterIncomeForPoint(points[i]);
                    }
                }

                //modifying water income accordingly to points on same levels
                for(int i = 0; i < pointsCount; ++i)
                {
                    if(points[i].OutputSameLevel.Count != 0)
                    {
                        double waterIncomePart = 
                            points[i].ThisPoint.WaterIncome / points[i].OutputSameLevel.Count;
                        for(int k = 0; k < points[i].OutputSameLevel.Count; ++k)
                        {
                            points[i].OutputSameLevel[k].ThisPoint.WaterIncome +=
                                waterIncomePart * WATER_INCOME_ADDITION_FOR_SAME_LVL_COEFFICIENT;
                        }
                    }
                }
            }
        }

        public LinkedPoint GetLongestRiverInStorage()
        {
            if(!isLinked)
            {
                throw new ArgumentException("Points weren't linked");
            }

            LinkedPoint res = points[0];
            LinkedPoint curr, currstart;
            int maxLengthCounter = 0;
            for(int i = 0; i < this.pointsCount; ++i)
            {
                currstart = curr = points[i];
                int currLengthCounter = 0;
                while(curr != null)
                {
                    curr = curr.Output;
                    ++currLengthCounter;
                }

                if(maxLengthCounter < currLengthCounter)
                {
                    maxLengthCounter = currLengthCounter;
                    res = currstart;
                }
            }

            return res;
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
            public List<LinkedPoint> OutputSameLevel { get; set; }
            public LinkedPoint Output { get; set; }
            public bool waterIncomeDefined = false;

            public LinkedPoint()
            {
                ThisPoint = new Point();
                Input = new List<LinkedPoint>();
                OutputSameLevel = new List<LinkedPoint>();
                Output = null;
            }

            public LinkedPoint(double x, double y, double height)
            {
                ThisPoint = new Point(x, y, height);
                Input = new List<LinkedPoint>();
                OutputSameLevel = new List<LinkedPoint>();
                Output = null;
            }
        }
    }
}
