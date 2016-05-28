using RiverMaker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RiverMaker.Controllers
{
    public class GridStorageController
    {
        private int b = 0, a = 0;
        public PointStorageBase storage;
        public GridStorageController(int numberOfPoints, Bitmap image)
        {
            b = (int)GetB(numberOfPoints, image.Width, image.Height);
            a = numberOfPoints / b;
            double gap = ((double)image.Width) / a;
            
            storage = new PointStorageBase(a * b);
            for(int i = 0; i < b; ++i)
            {
                for(int j = 0; j < a; ++j)
                {
                    storage.points[i * a + j] =
                        new PointStorageBase.LinkedPoint((j * gap) / image.Width, (i * gap) / image.Height,
                                                            HeightDefiner.GetBlackHeight(image.GetPixel((int)(j*gap), (int)(i*gap))));
                }
            }
        }

        public void LinkPoints()
        {
            double minHeight = 1.0;
            double currPointHeight;
            PointStorageBase.LinkedPoint pointToLook = null,
                lowestPoint = null,
                currPoint = null;
            for (int i = 0; i < b; ++i)
            {
                for(int j = 0; j < a; ++j)
                {
                    minHeight = 1.0;
                    lowestPoint = null;
                    currPoint = storage.points[i * a + j];
                    currPointHeight = currPoint.ThisPoint.Height;
                    if(i > 0)
                    {
                        if(j > 0)
                        {
                            pointToLook = storage.points[(i - 1) * a + j-1];
                            if (pointToLook.ThisPoint.Height <= minHeight
                                && pointToLook.ThisPoint.Height < currPointHeight)
                            {
                                lowestPoint = pointToLook;
                                minHeight = pointToLook.ThisPoint.Height;
                            }
                        }

                        pointToLook = storage.points[(i - 1) * a + j];
                        if(pointToLook.ThisPoint.Height <= minHeight 
                            && pointToLook.ThisPoint.Height < currPointHeight)
                        {
                            lowestPoint = pointToLook;
                            minHeight = pointToLook.ThisPoint.Height;
                        }
                    }

                    if(j > 0)
                    {
                        if (i < b-1)
                        {
                            pointToLook = storage.points[(i + 1) * a + j - 1];
                            if (pointToLook.ThisPoint.Height <= minHeight
                                && pointToLook.ThisPoint.Height < currPointHeight)
                            {
                                lowestPoint = pointToLook;
                                minHeight = pointToLook.ThisPoint.Height;
                            }
                        }

                        pointToLook = storage.points[i * a + j - 1];
                        if (pointToLook.ThisPoint.Height <= minHeight
                            && pointToLook.ThisPoint.Height < currPointHeight)
                        {
                            lowestPoint = pointToLook;
                            minHeight = pointToLook.ThisPoint.Height;
                        }
                    }

                    if(i < b-1)
                    {
                        if (j < a-1)
                        {
                            pointToLook = storage.points[(i + 1) * a + j + 1];
                            if (pointToLook.ThisPoint.Height <= minHeight
                                && pointToLook.ThisPoint.Height < currPointHeight)
                            {
                                lowestPoint = pointToLook;
                                minHeight = pointToLook.ThisPoint.Height;
                            }
                        }

                        pointToLook = storage.points[(i + 1) * a + j];
                        if (pointToLook.ThisPoint.Height <= minHeight
                            && pointToLook.ThisPoint.Height < currPointHeight)
                        {
                            lowestPoint = pointToLook;
                            minHeight = pointToLook.ThisPoint.Height;
                        }
                    }

                    if(j < a-1)
                    {
                        if (i > 0)
                        {
                            pointToLook = storage.points[(i - 1) * a + j + 1];
                            if (pointToLook.ThisPoint.Height <= minHeight
                                && pointToLook.ThisPoint.Height < currPointHeight)
                            {
                                lowestPoint = pointToLook;
                                minHeight = pointToLook.ThisPoint.Height;
                            }
                        }

                        pointToLook = storage.points[i * a + j + 1];
                        if (pointToLook.ThisPoint.Height <= minHeight
                            && pointToLook.ThisPoint.Height < currPointHeight)
                        {
                            lowestPoint = pointToLook;
                            minHeight = pointToLook.ThisPoint.Height;
                        }
                    }

                    if(lowestPoint != null)
                    {
                        currPoint.Output = lowestPoint;
                        lowestPoint.Input.Add(currPoint);
                    }
                }
            }

            LinkSameLevelPoints();

            storage.isLinked = true;
        }

        public void LinkSameLevelPoints()
        {
            for(int i = 0; i < b; ++i)
            {
                for(int j = 0; j < a; ++j)
                {
                    var CurrPoint = storage.points[i * a + j];
                    PointStorageBase.LinkedPoint pointToLook;
                    //works only for low points.
                    if(CurrPoint.Output == null)
                    {
                        if (i > 0)
                        {
                            if (j > 0)
                            {
                                pointToLook = storage.points[(i - 1) * a + j - 1];
                                if (pointToLook.ThisPoint.Height 
                                    == CurrPoint.ThisPoint.Height)
                                {
                                    CurrPoint.OutputSameLevel.Add(pointToLook);
                                }
                            }

                            pointToLook = storage.points[(i - 1) * a + j];
                            if (pointToLook.ThisPoint.Height 
                                == CurrPoint.ThisPoint.Height)
                            {
                                CurrPoint.OutputSameLevel.Add(pointToLook);
                            }
                        }

                        if (j > 0)
                        {
                            if (i < b - 1)
                            {
                                pointToLook = storage.points[(i + 1) * a + j - 1];
                                if (pointToLook.ThisPoint.Height
                                    == CurrPoint.ThisPoint.Height)
                                {
                                    CurrPoint.OutputSameLevel.Add(pointToLook);
                                }
                            }

                            pointToLook = storage.points[i * a + j - 1];
                            if (pointToLook.ThisPoint.Height
                                    == CurrPoint.ThisPoint.Height)
                            {
                                CurrPoint.OutputSameLevel.Add(pointToLook);
                            }
                        }

                        if (i < b - 1)
                        {
                            if (j < a - 1)
                            {
                                pointToLook = storage.points[(i + 1) * a + j + 1];
                                if (pointToLook.ThisPoint.Height
                                    == CurrPoint.ThisPoint.Height)
                                {
                                    CurrPoint.OutputSameLevel.Add(pointToLook);
                                }
                            }

                            pointToLook = storage.points[(i + 1) * a + j];
                            if (pointToLook.ThisPoint.Height
                                    == CurrPoint.ThisPoint.Height)
                            {
                                CurrPoint.OutputSameLevel.Add(pointToLook);
                            }
                        }

                        if (j < a - 1)
                        {
                            if (i > 0)
                            {
                                pointToLook = storage.points[(i - 1) * a + j + 1];
                                if (pointToLook.ThisPoint.Height
                                    == CurrPoint.ThisPoint.Height)
                                {
                                    CurrPoint.OutputSameLevel.Add(pointToLook);
                                }
                            }

                            pointToLook = storage.points[i * a + j + 1];
                            if (pointToLook.ThisPoint.Height
                                    == CurrPoint.ThisPoint.Height)
                            {
                                CurrPoint.OutputSameLevel.Add(pointToLook);
                            }
                        }
                    }
                }
            }
        }

        private double GetB(int numberOfPoints, int width, int height)
        {
            double discr = Math.Sqrt(
                Math.Pow(width - height, 2) + 4.0 * width * height * numberOfPoints);
            double b1 = ((height - width) + discr) / (2.0*width);
            double b2 = ((height - width) - discr) / (2.0 * width);
            if (b1 > 0 && b1 < numberOfPoints)
            {
                return b1;
            }
            else if (b2 > 0 && b2 < numberOfPoints)
            {
                return b2;
            }
            else
                throw new ArgumentException("can't find appropriate b");
        }
        
    }
}
