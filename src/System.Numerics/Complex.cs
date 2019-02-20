// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Numerics {

    [StructLayout(LayoutKind.Sequential)]
    public struct Complex :
    IEquatable<Complex>,
    IFormattable {
        private const double LOG_10_INV = 0.43429448190325;
        public static readonly Complex Zero;
        public static readonly Complex One;
        public static readonly Complex ImaginaryOne;

        public double Real { get; }

        public double Imaginary { get; }

        public double Magnitude => Abs(this);

        public double Phase => Math.Atan2(Imaginary, Real);

        public Complex(double real, double imaginary) {
            Real = real;
            Imaginary = imaginary;
        }

        public static Complex FromPolarCoordinates(double magnitude, double phase) {
            return new Complex(magnitude * Math.Cos(phase), magnitude * Math.Sin(phase));
        }

        public static Complex Negate(Complex value) => -value;

        public static Complex Add(Complex left, Complex right) => (left + right);

        public static Complex Subtract(Complex left, Complex right) => (left - right);

        public static Complex Multiply(Complex left, Complex right) => (left * right);

        public static Complex Divide(Complex dividend, Complex divisor) => (dividend / divisor);

        public static Complex operator -(Complex value) => new Complex(-value.Real, -value.Imaginary);

        public static Complex operator +(Complex left, Complex right) => new Complex(left.Real + right.Real, left.Imaginary + right.Imaginary);

        public static Complex operator -(Complex left, Complex right) => new Complex(left.Real - right.Real, left.Imaginary - right.Imaginary);

        public static Complex operator *(Complex left, Complex right) {
            double real = (left.Real * right.Real) - (left.Imaginary * right.Imaginary);
            return new Complex(real, (left.Imaginary * right.Real) + (left.Real * right.Imaginary));
        }

        public static Complex operator /(Complex left, Complex right) {
            double real = left.Real;
            double imaginary = left.Imaginary;
            double num3 = right.Real;
            double num4 = right.Imaginary;
            if (Math.Abs(num4) < Math.Abs(num3)) {
                return new Complex((real + (imaginary * (num4 / num3))) / (num3 + (num4 * (num4 / num3))), (imaginary - (real * (num4 / num3))) / (num3 + (num4 * (num4 / num3))));
            }
            return new Complex((imaginary + (real * (num3 / num4))) / (num4 + (num3 * (num3 / num4))), (-real + (imaginary * (num3 / num4))) / (num4 + (num3 * (num3 / num4))));
        }

        public static double Abs(Complex value) {
            if (double.IsInfinity(value.Real) || double.IsInfinity(value.Imaginary)) {
                return double.PositiveInfinity;
            }
            double num = Math.Abs(value.Real);
            double num2 = Math.Abs(value.Imaginary);
            if (num > num2) {
                double num3 = num2 / num;
                return (num * Math.Sqrt(1.0 + (num3 * num3)));
            }
            if (num2 == 0.0) {
                return num;
            }
            double num4 = num / num2;
            return (num2 * Math.Sqrt(1.0 + (num4 * num4)));
        }

        public static Complex Conjugate(Complex value) => new Complex(value.Real, -value.Imaginary);

        public static Complex Reciprocal(Complex value) => (value.Real == 0.0) && (value.Imaginary == 0.0) ? Zero : One / value;

        public static bool operator ==(Complex left, Complex right) => ((left.Real == right.Real) && (left.Imaginary == right.Imaginary));

        public static bool operator !=(Complex left, Complex right) => left.Real == right.Real ? !(left.Imaginary == right.Imaginary) : true;

        public override bool Equals(object obj) => ((obj is Complex) && (this == ((Complex)obj)));

        public bool Equals(Complex value) => Real.Equals(value.Real) && Imaginary.Equals(value.Imaginary);

        public static implicit operator Complex(short value) => new Complex(value, 0.0);

        public static implicit operator Complex(int value) => new Complex(value, 0.0);

        public static implicit operator Complex(long value) => new Complex(value, 0.0);

        public static implicit operator Complex(ushort value) => new Complex(value, 0.0);

        public static implicit operator Complex(uint value) => new Complex(value, 0.0);

        public static implicit operator Complex(ulong value) => new Complex(value, 0.0);

        public static implicit operator Complex(sbyte value) => new Complex(value, 0.0);

        public static implicit operator Complex(byte value) => new Complex(value, 0.0);

        public static implicit operator Complex(float value) => new Complex(value, 0.0);

        public static implicit operator Complex(double value) => new Complex(value, 0.0);

        public static explicit operator Complex(BigInteger value) => new Complex((double)value, 0.0);

        public static explicit operator Complex(decimal value) => new Complex((double)value, 0.0);

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "({0}, {1})", new object[] { Real, Imaginary });

        public string ToString(string format) => string.Format(CultureInfo.CurrentCulture, "({0}, {1})", new object[] { Real.ToString(format, CultureInfo.CurrentCulture), Imaginary.ToString(format, CultureInfo.CurrentCulture) });

        public string ToString(IFormatProvider provider) => string.Format(provider, "({0}, {1})", new object[] { Real, Imaginary });

        public string ToString(string format, IFormatProvider provider) => string.Format(provider, "({0}, {1})", new object[] { Real.ToString(format, provider), Imaginary.ToString(format, provider) });

        public override int GetHashCode() {
            int num = 0x5f5e0fd;
            int num2 = Real.GetHashCode() % num;
            int hashCode = Imaginary.GetHashCode();
            return (num2 ^ hashCode);
        }

        public static Complex Sin(Complex value) {
            double real = value.Real;
            double imaginary = value.Imaginary;
            return new Complex(Math.Sin(real) * Math.Cosh(imaginary), Math.Cos(real) * Math.Sinh(imaginary));
        }

        public static Complex Sinh(Complex value) {
            double real = value.Real;
            double imaginary = value.Imaginary;
            return new Complex(Math.Sinh(real) * Math.Cos(imaginary), Math.Cosh(real) * Math.Sin(imaginary));
        }

        public static Complex Asin(Complex value) {
            return (-ImaginaryOne * Log((ImaginaryOne * value) + Sqrt(One - (value * value))));
        }

        public static Complex Cos(Complex value) {
            double real = value.Real;
            double imaginary = value.Imaginary;
            return new Complex(Math.Cos(real) * Math.Cosh(imaginary), -(Math.Sin(real) * Math.Sinh(imaginary)));
        }

        public static Complex Cosh(Complex value) {
            double real = value.Real;
            double imaginary = value.Imaginary;
            return new Complex(Math.Cosh(real) * Math.Cos(imaginary), Math.Sinh(real) * Math.Sin(imaginary));
        }

        public static Complex Acos(Complex value) => (-ImaginaryOne * Log(value + (ImaginaryOne * Sqrt(One - (value * value)))));

        public static Complex Tan(Complex value) => (Sin(value) / Cos(value));

        public static Complex Tanh(Complex value) => (Sinh(value) / Cosh(value));

        public static Complex Atan(Complex value) {
            Complex complex = new Complex(2.0, 0.0);
            return ((ImaginaryOne / complex) * (Log(One - (ImaginaryOne * value)) - Log(One + (ImaginaryOne * value))));
        }

        public static Complex Log(Complex value) => new Complex(Math.Log(Abs(value)), Math.Atan2(value.Imaginary, value.Real));

        public static Complex Log(Complex value, double baseValue) => (Log(value) / Log(baseValue));

        public static Complex Log10(Complex value) => Scale(Log(value), 0.43429448190325);

        public static Complex Exp(Complex value) {
            double num = Math.Exp(value.Real);
            double real = num * Math.Cos(value.Imaginary);
            return new Complex(real, num * Math.Sin(value.Imaginary));
        }

        public static Complex Sqrt(Complex value) => FromPolarCoordinates(Math.Sqrt(value.Magnitude), value.Phase / 2.0);

        public static Complex Pow(Complex value, Complex power) {
            if (power == Zero) {
                return One;
            }
            if (value == Zero) {
                return Zero;
            }
            double real = value.Real;
            double imaginary = value.Imaginary;
            double y = power.Real;
            double num4 = power.Imaginary;
            double d = Abs(value);
            double num6 = Math.Atan2(imaginary, real);
            double num7 = (y * num6) + (num4 * Math.Log(d));
            double num8 = Math.Pow(d, y) * Math.Pow(2.7182818284590451, -num4 * num6);
            return new Complex(num8 * Math.Cos(num7), num8 * Math.Sin(num7));
        }

        public static Complex Pow(Complex value, double power) => Pow(value, new Complex(power, 0.0));

        private static Complex Scale(Complex value, double factor) {
            double real = factor * value.Real;
            return new Complex(real, factor * value.Imaginary);
        }

        static Complex() {
            Zero = new Complex(0.0, 0.0);
            One = new Complex(1.0, 0.0);
            ImaginaryOne = new Complex(0.0, 1.0);
        }
    }
}

