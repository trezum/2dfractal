using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _2dFractal
{
    public class CalculationService
    {
        private readonly int _width;
        private readonly int _widthHalf;
        private readonly int _height;
        private readonly int _heightHalf;
        private readonly int _iterations;

        public CalculationService(int width, int height)
        {
            _width = width;
            _widthHalf = width / 2;
            _height = height;
            _heightHalf = height / 2;
            _iterations = 10;
        }

        public Color GetPointColour(Point point)
        {
            Point p = TranslatePoint(point);

            if (p.X*p.X + p.Y*p.Y > 4)
            {
                return Color.FromRgb(0, 0, 0);
            }
            if (IsPointInSet(p))
            {
                return Color.FromRgb(0, 0, 0);
            }

            


            return Color.FromRgb(200, 0, 0);

            
        }

        private bool IsPointInSet(Point point)
        {
            var xZ = DoIteration(0, point.X);
            var yZ = DoIteration(0, point.Y);
            for (int i = 0; i < _iterations; i++)
            {
                xZ = DoIteration(xZ, point.X);
                yZ = DoIteration(yZ, point.Y);

                if (xZ*xZ + yZ*yZ < 4)
                {
                    return true;
                }
            }
            return false;
        }

        //z is the result, length of the imaginary number.
        //c is the input to the function
        private double DoIteration(double z, double c)
        {
            return z * z + c;
        }

    
        


        private Point TranslatePoint(Point p)
        {
            double y;
            double x;

            if (p.X > _widthHalf)
            {
                x = (p.X - _widthHalf) / _widthHalf * 2;
            }
            else
            {
                x = (p.X - _widthHalf) / _widthHalf * 2 *-1;
            }

            if (p.Y > _heightHalf)
            {
                y = (p.Y - _heightHalf) / _heightHalf * 2;
            }
            else
            {
                y = (p.Y - _heightHalf) / _heightHalf * 2 * -1;
            }

            return new Point(x,y);
        }

    }
}
