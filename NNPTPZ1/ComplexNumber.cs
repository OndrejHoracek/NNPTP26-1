using System;

namespace NNPTPZ1
{

    namespace Mathematics
    {
        public class ComplexNumber
        {
            public double RealPart { get; set; }
            public double ImaginariPart { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is ComplexNumber)
                {
                    ComplexNumber x = obj as ComplexNumber;
                    return x.RealPart == RealPart && x.ImaginariPart == ImaginariPart;
                }
                return base.Equals(obj);
            }

            public readonly static ComplexNumber Zero = new ComplexNumber()
            {
                RealPart = 0,
                ImaginariPart = 0
            };

            public ComplexNumber Multiply(ComplexNumber b)
            {
                ComplexNumber a = this;
                // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
                return new ComplexNumber()
                {
                    RealPart = a.RealPart * b.RealPart - a.ImaginariPart * b.ImaginariPart,
                    ImaginariPart = (a.RealPart * b.ImaginariPart + a.ImaginariPart * b.RealPart)
                };
            }
            public double GetAbs()
            {
                return Math.Sqrt( RealPart * RealPart + ImaginariPart * ImaginariPart);
            }

            public ComplexNumber Add(ComplexNumber b)
            {
                ComplexNumber a = this;
                return new ComplexNumber()
                {
                    RealPart = a.RealPart + b.RealPart,
                    ImaginariPart = a.ImaginariPart + b.ImaginariPart
                };
            }
            public double GetAngleInDegrees()
            {
                return Math.Atan(ImaginariPart / RealPart);
            }
            public ComplexNumber Subtract(ComplexNumber b)
            {
                ComplexNumber a = this;
                return new ComplexNumber()
                {
                    RealPart = a.RealPart - b.RealPart,
                    ImaginariPart = a.ImaginariPart - b.ImaginariPart
                };
            }

            public override string ToString()
            {
                return $"({RealPart} + {ImaginariPart}i)";
            }

            internal ComplexNumber Divide(ComplexNumber b)
            {
                // (aRe + aIm*i) / (bRe + bIm*i)
                // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
                //  bRe*bRe - bIm*bIm*i*i
                var tmp = this.Multiply(new ComplexNumber() { RealPart = b.RealPart, ImaginariPart = -b.ImaginariPart });
                var tmp2 = b.RealPart * b.RealPart + b.ImaginariPart * b.ImaginariPart;

                return new ComplexNumber()
                {
                    RealPart = tmp.RealPart / tmp2,
                    ImaginariPart = (tmp.ImaginariPart / tmp2)
                };
            }
        }
    }
}
