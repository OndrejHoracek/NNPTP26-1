using System;

namespace NNPTPZ1.Mathematics
{
    public class ComplexNumber
    {
        public double RealPart { get; set; }
        public double ImaginaryPart { get; set; }


        public readonly static ComplexNumber Zero = new ComplexNumber()
        {
            RealPart = 0,
            ImaginaryPart = 0
        };



        public ComplexNumber Add(ComplexNumber addend)
        {
            return new ComplexNumber()
            {
                RealPart = RealPart + addend.RealPart,
                ImaginaryPart = ImaginaryPart + addend.ImaginaryPart
            };
        }

        public ComplexNumber Subtract(ComplexNumber subtrahend)
        {
            return new ComplexNumber()
            {
                RealPart = RealPart - subtrahend.RealPart,
                ImaginaryPart = ImaginaryPart - subtrahend.ImaginaryPart
            };
        }

        public ComplexNumber Multiply(ComplexNumber multiplier)
        {
            return new ComplexNumber()
            {
                RealPart = RealPart * multiplier.RealPart - ImaginaryPart * multiplier.ImaginaryPart,
                ImaginaryPart = (RealPart * multiplier.ImaginaryPart + ImaginaryPart * multiplier.RealPart)
            };
        }

        internal ComplexNumber Divide(ComplexNumber divider)
        {
            var numerator = Multiply(new ComplexNumber() { RealPart = divider.RealPart, ImaginaryPart = -divider.ImaginaryPart });
            var denominator = divider.RealPart * divider.RealPart + divider.ImaginaryPart * divider.ImaginaryPart;

            return new ComplexNumber()
            {
                RealPart = numerator.RealPart / denominator,
                ImaginaryPart = (numerator.ImaginaryPart / denominator)
            };
        }


        public double GetAbs()
        {
            return Math.Sqrt(RealPart * RealPart + ImaginaryPart * ImaginaryPart);
        }

        public double GetAngleInRadians()
        {
            return Math.Atan(ImaginaryPart / RealPart);
        }



        public override bool Equals(object obj)
        {
            if (obj is ComplexNumber)
            {
                ComplexNumber compared = obj as ComplexNumber;
                return compared.RealPart == RealPart && compared.ImaginaryPart == ImaginaryPart;
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return $"({RealPart} + {ImaginaryPart}i)";
        }

    }
}
