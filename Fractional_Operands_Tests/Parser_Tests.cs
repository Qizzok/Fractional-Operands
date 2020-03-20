using System;
using System.Collections.Generic;
using System.Text;
using Fractional_Operands; 
using NUnit.Framework;

namespace Fractional_Operands_Tests
{
    public class Parser_Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestCalculateAdds() {
            List<Operation> ops = new List<Operation> { Operation.Addition };
            List<Fraction> vals = new List<Fraction> { new Fraction(1, 2), new Fraction(2, 3) };
            Fraction result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1_1/6"));

            ops = new List<Operation> { Operation.Addition, Operation.Addition };
            vals = new List<Fraction> { new Fraction(1, 2), new Fraction(2, 3), new Fraction(1, 4) };
            result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1_5/12"));
        }

        [Test]
        public void TestCalculateSubtracts() {
            List<Operation> ops = new List<Operation> { Operation.Subtraction };
            List<Fraction> vals = new List<Fraction> { new Fraction(2, 3), new Fraction(1, 2) };
            Fraction result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1/6"));

            ops = new List<Operation> { Operation.Subtraction, Operation.Addition };
            vals = new List<Fraction> { new Fraction(1, 2), new Fraction(2, 3), new Fraction(1, 4) };
            result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1/12"));
        }

        [Test]
        public void TestCalculateMultiplies() {
            List<Operation> ops = new List<Operation> { Operation.Multiplication };
            List<Fraction> vals = new List<Fraction> { new Fraction(2, 3), new Fraction(1, 2) };
            Fraction result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1/3"));

            ops = new List<Operation> { Operation.Subtraction, Operation.Multiplication };
            vals = new List<Fraction> { new Fraction(1, 2), new Fraction(2, 3), new Fraction(1, 4) };
            result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1/3"));
        }

        [Test]
        public void TestCalculateDivides() {
            List<Operation> ops = new List<Operation> { Operation.Division };
            List<Fraction> vals = new List<Fraction> { new Fraction(2, 3), new Fraction(1, 2) };
            Fraction result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1_1/3"));

            ops = new List<Operation> { Operation.Addition, Operation.Division };
            vals = new List<Fraction> { new Fraction(1, 2), new Fraction(3, 4), new Fraction(2, 3) };
            result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1_5/8"));
        }

        [Test]
        public void TestCalculateRespectsOrder() {
            List<Operation> ops = new List<Operation> { Operation.Addition, Operation.Multiplication };
            List<Fraction> vals = new List<Fraction> { new Fraction(1, 2), new Fraction(2, 3), new Fraction(3, 4) };
            Fraction result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1"));

            ops = new List<Operation> { Operation.Addition, Operation.Multiplication };
            vals = new List<Fraction> { new Fraction(2, 3), new Fraction(1, 2), new Fraction(3, 4) };
            result = Parser.Calculate(ops, vals);
            Assert.That(result.ToString(), Is.EqualTo("1_1/24"));
        }

        [Test]
        public void TestParsingThrowsWhenInvalid() {
            var ex = Assert.Throws<ArgumentException>(() => Parser.ParseAndCalculate(new string[] { }));
            Assert.That(ex.Message, Is.EqualTo("Must provide at least one value to evaluate"));
            ex = Assert.Throws<ArgumentException>(() => Parser.ParseAndCalculate("".Split(' ')));
            Assert.That(ex.Message, Is.EqualTo("Non-integer values passed"));
            ex = Assert.Throws<ArgumentException>(() => Parser.ParseAndCalculate("2/3 1/4".Split(' ')));
            Assert.That(ex.Message, Is.EqualTo("Operator/Operand mismatch"));
            ex = Assert.Throws<ArgumentException>(() => Parser.ParseAndCalculate("+ 2/3 - 1/4".Split(' ')));
            Assert.That(ex.Message, Is.EqualTo("Operator/Operand mismatch"));
            ex = Assert.Throws<ArgumentException>(() => Parser.ParseAndCalculate("2/3 / 1/4 *".Split(' ')));
            Assert.That(ex.Message, Is.EqualTo("Operator/Operand mismatch"));
            ex = Assert.Throws<ArgumentException>(() => Parser.ParseAndCalculate("2/3 1/4 *".Split(' ')));
            Assert.That(ex.Message, Is.EqualTo("Operator/Operand mismatch"));
        }

        [Test]
        public void TestParsingWorksWhenValid() {
            Fraction result = Parser.ParseAndCalculate("2/3 / 1/2".Split(' '));
            Assert.That(result.ToString(), Is.EqualTo("1_1/3"));
            result = Parser.ParseAndCalculate("1/2 + 3/4 / 2/3".Split(' '));
            Assert.That(result.ToString(), Is.EqualTo("1_5/8"));
            result = Parser.ParseAndCalculate("-2 / 1_1/2 - 1_2/3 * -1/2".Split(' '));
            Assert.That(result.ToString(), Is.EqualTo("-1/2"));
            result = Parser.ParseAndCalculate("-2_1/2".Split(' '));
            Assert.That(result.ToString(), Is.EqualTo("-2_1/2"));
        }
    }
}
