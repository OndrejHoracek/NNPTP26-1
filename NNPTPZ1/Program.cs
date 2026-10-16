using System;
using System.Collections.Generic;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    class Program
    { 
        private const int MaxIterations = 30;
        private const double ConvergenceThreshold = 0.5;
        private const double RootTolerance = 0.01;
        private const double ZeroReplacement = 0.0001;
        private const int IterationDarkening = 2;

        private static readonly Color[] Colors =
        {
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
        };


        public static void Main(string[] args)
        {
            int width = int.Parse(args[0]);
            int height = int.Parse(args[1]);

            double xMin = double.Parse(args[2]);
            double xMax = double.Parse(args[3]);
            double yMin = double.Parse(args[4]);
            double yMax = double.Parse(args[5]);

            string output = args[6];

            double xStep = (xMax - xMin) / width;
            double yStep = (yMax - yMin) / height;

            List<ComplexNumber> roots = new List<ComplexNumber>();
            Polynomial polynomial = CreatePolynom();
            Polynomial derivative = polynomial.Derive();


            Console.WriteLine(polynomial);
            Console.WriteLine(derivative);


            Bitmap bitmap = CreateBitmap(width, height, xMin, yMin,
                xStep, yStep, roots, polynomial, derivative, Colors);

            bitmap.Save(output ?? "../../../out.png");
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

        private static Bitmap CreateBitmap(int width, int height, double xMin, double yMin, double xStep, double yStep, List<ComplexNumber> roots, Polynomial polynomial, Polynomial derivative, Color[] colors)
        {
            Bitmap bitmap = new Bitmap(width, height);

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

            return bitmap;
        }

        private static ComplexNumber CreatePoint(double x, double y)
        {
            ComplexNumber point = new ComplexNumber()
            {
                RealPart = x,
                ImaginaryPart = y
            };

            if (point.RealPart == 0)
                point.RealPart = ZeroReplacement;

            if (point.ImaginaryPart == 0)
                point.ImaginaryPart = ZeroReplacement;

            return point;
        }


        private static ComplexNumber ApplyNewtonItteration(Polynomial polynomial, Polynomial derivative, ComplexNumber current, out int iteration)
        {
            iteration = 0;
            for (int i = 0; i < MaxIterations; i++)
            {
                var difference = polynomial.Evaluate(current).Divide(derivative.Evaluate(current));
                current = current.Subtract(difference);

                if (Math.Pow(difference.RealPart, 2) + Math.Pow(difference.ImaginaryPart, 2) >= ConvergenceThreshold)
                {
                    i--;
                }
                iteration++;
            }

            return current;
        }


        private static int FindOrAddRootIndex(List<ComplexNumber> roots, ComplexNumber current)
        {
            var indexOfRoot = 0;
            var knownRoot = false;
            for (int potentialRoot = 0; potentialRoot < roots.Count; potentialRoot++)
            {
                if (Math.Pow(current.RealPart - roots[potentialRoot].RealPart, 2) + Math.Pow(current.ImaginaryPart - roots[potentialRoot].ImaginaryPart, 2) <= RootTolerance)
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

        

        private static Color GetColor(Color[] colors, int iteration, int rootIndex)
        {
            var color = colors[rootIndex % colors.Length];
            color = Color.FromArgb(color.R, color.G, color.B);
            color = Color.FromArgb(Math.Min(Math.Max(0, color.R - iteration * IterationDarkening), 255),
                Math.Min(Math.Max(0, color.G - iteration * IterationDarkening), 255),
                Math.Min(Math.Max(0, color.B - iteration * IterationDarkening), 255));

            return color;
        }

        

        
    }
}
