using System.Collections.Generic;

namespace NNPTPZ1.Mathematics
{
    public class Polynomial
    {
        public List<ComplexNumber> Coefficients { get; set; }


        public Polynomial() => Coefficients = new List<ComplexNumber>();

        public void Add(ComplexNumber coefficient) => Coefficients.Add(coefficient);


        public Polynomial Derive()
        {
            Polynomial derivative = new Polynomial();
            for (int power = 1; power < Coefficients.Count; power++)
            {
                derivative.Add(Coefficients[power].Multiply(new ComplexNumber() { RealPart = power }));
            }

            return derivative;
        }


        public ComplexNumber Evaluate(double x)
        {
            return Evaluate(new ComplexNumber() { RealPart = x, ImaginaryPart = 0 });
        }


        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber result = ComplexNumber.Zero;
            for (int power = 0; power < Coefficients.Count; power++)
            {
                ComplexNumber coefficient = Coefficients[power];
                ComplexNumber powerOfX = x;

                if (power > 0)
                {
                    for (int i = 0; i < power - 1; i++)
                        powerOfX = powerOfX.Multiply(x);

                    coefficient = coefficient.Multiply(powerOfX);
                }

                result = result.Add(coefficient);
            }

            return result;
        }

        public override string ToString()
        {
            string result = "";
            for (int power = 0; power < Coefficients.Count; power++)
            {
                result += Coefficients[power];
                if (power > 0)
                {
                    for (int i = 0; i < power; i++)
                    {
                        result += "x";
                    }
                }
                if (power + 1 < Coefficients.Count)
                    result += " + ";
            }
            return result;
        }
    }

}
