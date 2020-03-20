using System;

namespace Fractional_Operands
{
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Usage: whole_numerator/denominator operand whole_numerator/denominator\n" +
                    "Valid operators are * / + -\n" +
                    "ex: 1/2 * 3_3/4");
                return;
            }

            try {
                Fraction result = Parser.ParseAndCalculate(args);
                Console.WriteLine("= {0}", result.ToString());
            } catch (Exception ex) {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}
