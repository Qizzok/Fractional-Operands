using System;

namespace Fractional_Operands
{
    public class Fraction
    {
        public int numerator { get; private set; }
        public int denominator { get; private set; }

        public Fraction(int numerator, int denominator) {
            this.numerator = numerator;
            this.denominator = denominator;
            if (denominator == 0) {
                throw new ArgumentException("Denominator cannot be zero");
            }
            Reduce();
        }

        /* Pseudocode to parse fraction from string
         * 
         * Split string around _
         *      More than two parts indicates error
         *      Two parts indicates whole part and fractional part
         *      One part indicates fractional part
         * Split fractional part around /
         *      More than two parts indicates error
         *      If two parts, parse as numerator and denominator
         *      If one part, parse as numerator with denominator 1
         *      If whole part exists and is negative, parse fractional part as negative as well
         * Multiply whole part by denominator and add to numerator
         * 
         * A single whole number (e.g. "3") will thus be parsed as X/1
         */

        public Fraction(string arg_string) { //Parse from CLI argument
            string[] parts = arg_string.Split('_');
            if (parts.Length > 2) {
                throw new ArgumentException("Invalid fractional format passed");
            }

            string[] fractional = parts[parts.Length - 1].Split('/'); //Last part will be fractional whether or not a whole part exists
            if (fractional.Length > 2) {
                throw new ArgumentException("Invalid fractional format passed");
            }

            int whole = 0;

            try {
                numerator = int.Parse(fractional[0]);
                denominator = fractional.Length == 2 ? int.Parse(fractional[1]) : 1;
                if (parts.Length == 2) { //If there is a whole part
                    whole = int.Parse(parts[0]);
                }
            } catch {
                throw new ArgumentException("Non-integer values passed");
            }

            if (denominator == 0) {
                throw new ArgumentException("Denominator cannot be zero");
            }

            if (whole != 0) { 
                if (whole < 0) {
                    numerator = -numerator; //Covers -3_3/4 case (-15/4)
                }
                numerator += whole * denominator;
            }

            Reduce();
        }

        private void Reduce() {
            if (denominator < 0) {
                denominator = -denominator;
                numerator = -numerator;
            }

            //Euclidean algorithm for greatest common divisor
            //https://en.wikipedia.org/wiki/Euclidean_algorithm
            int a = Math.Abs(numerator), b = denominator;
            while (a != 0 && b != 0) {
                if (a > b) {
                    a %= b;
                } else {
                    b %= a;
                }
            }

            numerator /= a == 0 ? b : a;
            denominator /= a == 0 ? b : a;
        }

        public override string ToString() {
            if (numerator == 0) {
                return "0";
            }
            int whole = numerator / denominator; //Integer division floors by default
            int num = numerator - (whole * denominator);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            if (whole != 0) {
                builder.Append(whole.ToString());
                if (num != 0) {
                    builder.Append("_");
                    num = Math.Abs(num); //negative has already been indicated
                }
            }
            if (num != 0) {
                builder.Append(num.ToString() + "/" + denominator.ToString());
            }
            return builder.ToString();
        }

        public static Fraction operator +(Fraction left, Fraction right) {
            int num1, num2, den;
            if (left.denominator == right.denominator) { //Convert to same denominator if necessary
                num1 = left.numerator;
                num2 = right.numerator;
                den = left.denominator; // == right.denominator
            } else {
                num1 = left.numerator * right.denominator;
                num2 = right.numerator * left.denominator;
                den = left.denominator * right.denominator;
            }
            return new Fraction(num1 + num2, den);
        }

        public static Fraction operator -(Fraction unary) {
            return new Fraction(-unary.numerator, unary.denominator);
        }

        public static Fraction operator -(Fraction left, Fraction right) {
            return left + -right; //Negate second operand and add
        }

        public static Fraction operator *(Fraction left, Fraction right) {
            return new Fraction(left.numerator * right.numerator, left.denominator * right.denominator);
        }

        public static Fraction operator /(Fraction left, Fraction right) {
            if (right.numerator == 0) {
                throw new InvalidOperationException("Dividend must not be zero");
            }
            return new Fraction(left.numerator * right.denominator, left.denominator * right.numerator);
        }
    }
}
