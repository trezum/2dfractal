using System;
using System.Numerics;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace _2dFractal
{
    public class CalculationService
    {
        private readonly int _widthHalf;
        private readonly int _heightHalf;
        private readonly int _iterations;

        public CalculationService(int width, int height)
        {
            _widthHalf = width / 2;
            _heightHalf = height / 2;
            _iterations = 40;
        }

        public Color GetPointColourOld(Point point)
        {
            Point p = TranslatePoint(point);

            if (IsPointInSet(p))
            {
                return Color.FromRgb(0, 0, 0);
            }
            return Color.FromRgb(200, 0, 0);
        }


        public Color GetPointColour(Point point)
        {
            Point p = TranslatePoint(point);


            var forEscape = IterationsForEscape(p);
            if (forEscape != _iterations && forEscape > 10)
            {
                var test = "";
            }
            var result = forEscape * 255 / _iterations;

            byte b = Convert.ToByte(result);
            
            return Color.FromRgb(b,b,b);
        }

        private int IterationsForEscape(Point point)
        {
            var input = new Complex(point.X, point.Y);
            var result = new Complex(0, 0);
            for (int i = 0; i < _iterations; i++)
            {
                if (result.Magnitude > 2)
                {
                    return i;
                }
                result = result * result + input;
            }
            return _iterations;
        }

        private bool IsPointInSet(Point point)
        {
            var input = new Complex(point.X,point.Y);
            var result = new Complex(0, 0);
            for (int i = 0; i < _iterations; i++)
            {
                if (result.Magnitude > 2)
                {
                    return false;
                }
                result = result * result + input;
            }
            return true;
        }




        private Point TranslatePoint(Point p)
        {
            return new Point((p.X - _widthHalf) / _widthHalf * 2, (p.Y - _heightHalf) / _heightHalf * 2);
        }
    }
}