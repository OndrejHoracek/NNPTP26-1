using System;
using System.Collections.Generic;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            int width = int.Parse(args[0]);
            int height = int.Parse(args[1]);

            double xMin = double.Parse(args[2]);
            double xMax = double.Parse(args[3]);
            double yMin = double.Parse(args[4]);
            double yMax = double.Parse(args[5]);

            string output = args[6];

            Bitmap bitmap = new Bitmap(width, height);

            double xStep = (xMax - xMin) / width;
            double yStep = (yMax - yMin) / height;

            List<ComplexNumber> roots = new List<ComplexNumber>();

            Polynomial polynomial = CreatePolynom();

            Polynomial derivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(derivative);

            var colors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };


            for (int yPixel = 0; yPixel < height; yPixel++)
            {
                for (int xPixel = 0; xPixel < width; xPixel++)
                {
                    double x = xMin + xPixel * xStep;
                    double y = yMin + yPixel * yStep;

                    ComplexNumber current = CreatePoint(x, y);

                    current = ApplyNewtonItteration(polynomial, derivative, current, out int iteration);

                    int indexOfRoot = FindOrAddRootIndex(roots, current);

                    Color color = GetColor(colors, iteration, indexOfRoot);

                    bitmap.SetPixel(xPixel, yPixel, color);
                }
            }



            bitmap.Save(output ?? "../../../out.png");

        }

        private static int FindOrAddRootIndex(List<ComplexNumber> roots, ComplexNumber current)
        {
            var indexOfRoot = 0;
            var knownRoot = false;
            for (int potentialRoot = 0; potentialRoot < roots.Count; potentialRoot++)
            {
                if (Math.Pow(current.RealPart - roots[potentialRoot].RealPart, 2) + Math.Pow(current.ImaginaryPart - roots[potentialRoot].ImaginaryPart, 2) <= 0.01)
                {
                    knownRoot = true;
                    indexOfRoot = potentialRoot;
                }
            }
            if (!knownRoot)
            {
                roots.Add(current);
                indexOfRoot = roots.Count;
            }

            return indexOfRoot;
        }

        private static ComplexNumber ApplyNewtonItteration(Polynomial polynomial, Polynomial derivative, ComplexNumber current, out int iteration)
        {
            iteration = 0;
            for (int i = 0; i < 30; i++)
            {
                var difference = polynomial.Evaluate(current).Divide(derivative.Evaluate(current));
                current = current.Subtract(difference);


                if (Math.Pow(difference.RealPart, 2) + Math.Pow(difference.ImaginaryPart, 2) >= 0.5)
                {
                    i--;
                }
                iteration++;
            }

            return current;
        }

        private static Color GetColor(Color[] colors, int iteration, int id)
        {
            var color = colors[id % colors.Length];
            color = Color.FromArgb(color.R, color.G, color.B);
            color = Color.FromArgb(Math.Min(Math.Max(0, color.R - iteration * 2), 255), Math.Min(Math.Max(0, color.G - (int)iteration * 2), 255), Math.Min(Math.Max(0, color.B - (int)iteration * 2), 255));
            return color;
        }

        private static ComplexNumber CreatePoint(double x, double y)
        {
            ComplexNumber point = new ComplexNumber()
            {
                RealPart = x,
                ImaginaryPart = y
            };

            if (point.RealPart == 0)
                point.RealPart = 0.0001;
            if (point.ImaginaryPart == 0)
                point.ImaginaryPart = 0.0001f;
            return point;
        }

        private static Polynomial CreatePolynom()
        {
            Polynomial polynomial = new Polynomial();
            polynomial.Add(new ComplexNumber() { RealPart = 1 });
            polynomial.Add(ComplexNumber.Zero);
            polynomial.Add(ComplexNumber.Zero);

            polynomial.Add(new ComplexNumber() { RealPart = 1 });
            return polynomial;
        }
    }
}
