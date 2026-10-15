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
        static void Main(string[] args)
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
            
            Polynomial polynomial = new Polynomial();
            polynomial.Add(new ComplexNumber() { RealPart = 1 });
            polynomial.Add(ComplexNumber.Zero);
            polynomial.Add(ComplexNumber.Zero);
            
            polynomial.Add(new ComplexNumber() { RealPart = 1 });
            Polynomial derivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(derivative);

            var colors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int yPixel = 0; yPixel < height; yPixel++)
            {
                for (int xPixel = 0; xPixel < width; xPixel++)
                {
                    // find "world" coordinates of pixel
                    double y = yMin + yPixel * yStep;
                    double x = xMin + xPixel * xStep;

                    ComplexNumber ox = new ComplexNumber()
                    {
                        RealPart = x,
                        ImaginaryPart = y
                    };

                    if (ox.RealPart == 0)
                        ox.RealPart = 0.0001;
                    if (ox.ImaginaryPart == 0)
                        ox.ImaginaryPart = 0.0001f;

                    //Console.WriteLine(ox);

                    // find solution of equation using newton's iteration
                    int iteration = 0;
                    for (int k = 0; k< 30; k++)
                    {
                        var difference = polynomial.Evaluate(ox).Divide(derivative.Evaluate(ox));
                        ox = ox.Subtract(difference);

                        //Console.WriteLine($"{q} {ox} -({diff})");
                        if (Math.Pow(difference.RealPart, 2) + Math.Pow(difference.ImaginaryPart, 2) >= 0.5)
                        {
                            k--;
                        }
                        iteration++;
                    }

                    //Console.ReadKey();

                    // find solution root number
                    var known = false;
                    var id = 0;
                    for (int w = 0; w <roots.Count;w++)
                    {
                        if (Math.Pow(ox.RealPart- roots[w].RealPart, 2) + Math.Pow(ox.ImaginaryPart - roots[w].ImaginaryPart, 2) <= 0.01)
                        {
                            known = true;
                            id = w;
                        }
                    }
                    if (!known)
                    {
                        roots.Add(ox);
                        id = roots.Count;
                        int maxid = id + 1;
                    }

                    // colorize pixel according to root number
                    //int vv = id;
                    //int vv = id * 50 + (int)it*5;
                    var vv = colors[id % colors.Length];
                    vv = Color.FromArgb(vv.R, vv.G, vv.B);
                    vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R - iteration * 2), 255), Math.Min(Math.Max(0, vv.G - (int)iteration*2), 255), Math.Min(Math.Max(0, vv.B - (int)iteration*2), 255));
                    //vv = Math.Min(Math.Max(0, vv), 255);
                    bitmap.SetPixel(xPixel, yPixel, vv);
                    //bmp.SetPixel(j, i, Color.FromArgb(vv, vv, vv));
                }
            }

          

                    bitmap.Save(output ?? "../../../out.png");

        }
    }
}
