using System;
using System.Collections.Generic;

namespace Fractional_Operands
{
    public enum Operation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    /* A slightly more elegant way of doing this would be to have a standalone function Parse, which would return a tree of objects which can resolve to a fraction.
     * These objects would be one of two implementations: a wrapper for a fraction or an operator with two child fraction resolveables.
     * This would allow parsing to be tested completely independently of calculation and would make it easier to add new operations,
     * but at the cost of dramatically increased complexity and additional loops through the arguments. I have chosen the simpler but slightly harder-to-test solution.
     * Were this behavior to be further extended or used elsewhere, the tree model for parsing should be made and used instead.
     */

    public class Parser
    {
        public static Fraction ParseAndCalculate(string[] args) {
            List<Operation> operators = new List<Operation>();
            List<Fraction> fractions = new List<Fraction>();

            bool expectFraction = true;

            for (int i = 0; i < args.Length; i++) {
                switch (args[i]) {
                case "+":
                    if (expectFraction) {
                        throw new ArgumentException("Operator/Operand mismatch");
                    }
                    operators.Add(Operation.Addition);
                    break;
                case "-":
                    if (expectFraction) {
                        throw new ArgumentException("Operator/Operand mismatch");
                    }
                    operators.Add(Operation.Subtraction);
                    break;
                case "*":
                    if (expectFraction) {
                        throw new ArgumentException("Operator/Operand mismatch");
                    }
                    operators.Add(Operation.Multiplication);
                    break;
                case "/":
                    if (expectFraction) {
                        throw new ArgumentException("Operator/Operand mismatch");
                    }
                    operators.Add(Operation.Division);
                    break;
                default: //Presumptive operand
                    if (!expectFraction) {
                        throw new ArgumentException("Operator/Operand mismatch");
                    }
                    fractions.Add(new Fraction(args[i]));
                    break;
                }
                expectFraction = !expectFraction;
            }

            return Calculate(operators, fractions);
        }

        public static Fraction Calculate(List<Operation> operators, List<Fraction> fractions) {
            if(fractions.Count == 0) {
                throw new ArgumentException("Must provide at least one value to evaluate");
            }
            if(fractions.Count <= operators.Count) {
                throw new ArgumentException("Operator/Operand mismatch");
            }

            int opIndex = 0;
            while (opIndex < operators.Count) {
                switch (operators[opIndex]) {
                case Operation.Multiplication:
                    Fraction product = fractions[opIndex] * fractions[opIndex + 1];
                    fractions.RemoveAt(opIndex);
                    fractions[opIndex] = product; //Remove one operand and replace the other before continuing
                    operators.RemoveAt(opIndex); //Remove the operator as it has been processed
                    break;
                case Operation.Division:
                    Fraction quotient = fractions[opIndex] / fractions[opIndex + 1];
                    fractions.RemoveAt(opIndex);
                    fractions[opIndex] = quotient;
                    operators.RemoveAt(opIndex);
                    break;
                default: //Do not process addition and subtraction yet to adhere to order of operations
                    opIndex++; //Leave the operator in place and continue
                    break;
                }
            }

            Fraction result = fractions[0];
            for (int i = 0; i < operators.Count; i++) { //Now process addition and subtraction
                if (operators[i] == Operation.Addition) {
                    result += fractions[i + 1];
                } else { //Subtration
                    result -= fractions[i + 1];
                }
            }

            return result;
        }
    }
}
