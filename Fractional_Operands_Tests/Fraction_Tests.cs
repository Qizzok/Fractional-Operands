using NUnit.Framework;
using Fractional_Operands;
using System;

namespace Fractional_Operands_Tests
{
    public class Fraction_Tests
    {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestFractionsParse() {
            Fraction f = new Fraction("4/3");
            Assert.That(f.numerator, Is.EqualTo(4));
            Assert.That(f.denominator, Is.EqualTo(3));
            f = new Fraction("3_1/5");
            Assert.That(f.numerator, Is.EqualTo(16));
            Assert.That(f.denominator, Is.EqualTo(5));
            f = new Fraction("-2_1/4");
            Assert.That(f.numerator, Is.EqualTo(-9));
            Assert.That(f.denominator, Is.EqualTo(4));
            f = new Fraction("-3");
            Assert.That(f.numerator, Is.EqualTo(-3));
            Assert.That(f.denominator, Is.EqualTo(1));
        }

        [Test]
        public void TestFractionsThrowWhenInvalid() {
            var ex = Assert.Throws<ArgumentException>(() => new Fraction("1_2_3"));
            Assert.That(ex.Message, Is.EqualTo("Invalid fractional format passed"));
            ex = Assert.Throws<ArgumentException>(() => new Fraction("1_2/3/4"));
            Assert.That(ex.Message, Is.EqualTo("Invalid fractional format passed"));
            ex = Assert.Throws<ArgumentException>(() => new Fraction("1_-3/4"));
            Assert.That(ex.Message, Is.EqualTo("Invalid fractional format passed"));
            ex = Assert.Throws<ArgumentException>(() => new Fraction("1/2_3/4"));
            Assert.That(ex.Message, Is.EqualTo("Non-integer values passed"));
            ex = Assert.Throws<ArgumentException>(() => new Fraction("_3/4"));
            Assert.That(ex.Message, Is.EqualTo("Non-integer values passed"));
            ex = Assert.Throws<ArgumentException>(() => new Fraction("1/0"));
            Assert.That(ex.Message, Is.EqualTo("Denominator cannot be zero"));
            ex = Assert.Throws<ArgumentException>(() => new Fraction(1, 0));
            Assert.That(ex.Message, Is.EqualTo("Denominator cannot be zero"));
            ex = Assert.Throws<ArgumentException>(() => new Fraction("foo_bar/baz"));
            Assert.That(ex.Message, Is.EqualTo("Non-integer values passed"));
        }

        [Test]
        public void TestFractionsStringify() {
            Fraction f = new Fraction(1, 2);
            Assert.That(f.ToString(), Is.EqualTo("1/2"));
            f = new Fraction(-1, 2);
            Assert.That(f.ToString(), Is.EqualTo("-1/2"));
            f = new Fraction(5, 2);
            Assert.That(f.ToString(), Is.EqualTo("2_1/2"));
            f = new Fraction(-4, 3);
            Assert.That(f.ToString(), Is.EqualTo("-1_1/3"));
            f = new Fraction(0, 1);
            Assert.That(f.ToString(), Is.EqualTo("0"));
        }

        [Test]
        public void TestFractionsReduce() {
            Fraction f = new Fraction(4, 6);
            Assert.That(f.ToString(), Is.EqualTo("2/3"));
            f = new Fraction(-4, 6);
            Assert.That(f.ToString(), Is.EqualTo("-2/3"));
            f = new Fraction("1_2/6");
            Assert.That(f.ToString(), Is.EqualTo("1_1/3"));
            f = new Fraction(1, -3);
            Assert.That(f.ToString(), Is.EqualTo("-1/3"));
        }

        [Test]
        public void TestFractionsAdd() {
            Fraction f1 = new Fraction(1, 3);
            Fraction f2 = new Fraction(1, 3);
            Fraction sum = f1 + f2;
            Assert.That(sum.ToString(), Is.EqualTo("2/3"));
            f1 = new Fraction(1, 3);
            f2 = new Fraction(2, 3);
            sum = f1 + f2;
            Assert.That(sum.ToString(), Is.EqualTo("1"));
            f1 = new Fraction(1, 3);
            f2 = new Fraction(1, 2);
            sum = f1 + f2;
            Assert.That(sum.ToString(), Is.EqualTo("5/6"));
            f1 = new Fraction(2, 3);
            f2 = new Fraction(1, -3);
            sum = f1 + f2;
            Assert.That(sum.ToString(), Is.EqualTo("1/3"));
        }

        [Test]
        public void TestFractionsNegate() {
            Fraction f = new Fraction(1, 2);
            f = -f;
            Assert.That(f.ToString(), Is.EqualTo("-1/2"));
            f = new Fraction(-4, 3);
            f = -f;
            Assert.That(f.ToString(), Is.EqualTo("1_1/3"));
        }

        [Test]
        public void TestFractionsSubtract() {
            Fraction f1 = new Fraction(1, 3);
            Fraction f2 = new Fraction(1, 3);
            Fraction diff = f1 - f2;
            Assert.That(diff.ToString(), Is.EqualTo("0"));
            f1 = new Fraction(1, 3);
            f2 = new Fraction(2, 3);
            diff = f1 - f2;
            Assert.That(diff.ToString(), Is.EqualTo("-1/3"));
            f1 = new Fraction(1, 3);
            f2 = new Fraction(1, 2);
            diff = f1 - f2;
            Assert.That(diff.ToString(), Is.EqualTo("-1/6"));
            f1 = new Fraction(2, 3);
            f2 = new Fraction(1, -3);
            diff = f1 - f2;
            Assert.That(diff.ToString(), Is.EqualTo("1"));
        }

        [Test]
        public void TestFractionsMultiply() {
            Fraction f1 = new Fraction(2, 3);
            Fraction f2 = new Fraction(2, 3);
            Fraction prod = f1 * f2;
            Assert.That(prod.ToString(), Is.EqualTo("4/9"));
            f1 = new Fraction(8, 3);
            f2 = new Fraction(1, 2);
            prod = f1 * f2;
            Assert.That(prod.ToString(), Is.EqualTo("1_1/3"));
            f1 = new Fraction(-2, 3);
            f2 = new Fraction(1, 2);
            prod = f1 * f2;
            Assert.That(prod.ToString(), Is.EqualTo("-1/3"));
            f1 = new Fraction(0, 3);
            f2 = new Fraction(1, 2);
            prod = f1 * f2;
            Assert.That(prod.ToString(), Is.EqualTo("0"));
        }

        [Test]
        public void TestFractionsDivide() {
            Fraction f1 = new Fraction(2, 3);
            Fraction f2 = new Fraction(2, 3);
            Fraction quot = f1 / f2;
            Assert.That(quot.ToString(), Is.EqualTo("1"));
            f1 = new Fraction(8, 3);
            f2 = new Fraction(1, 2);
            quot = f1 / f2;
            Assert.That(quot.ToString(), Is.EqualTo("5_1/3"));
            f1 = new Fraction(-2, 3);
            f2 = new Fraction(3, 4);
            quot = f1 / f2;
            Assert.That(quot.ToString(), Is.EqualTo("-8/9"));
            f1 = new Fraction(0, 3);
            f2 = new Fraction(1, 2);
            quot = f1 / f2;
            Assert.That(quot.ToString(), Is.EqualTo("0"));
            f1 = new Fraction(1, 2);
            f2 = new Fraction(-2, 1);
            quot = f1 / f2;
            Assert.That(quot.ToString(), Is.EqualTo("-1/4"));
            f1 = new Fraction(1, 1);
            f2 = new Fraction(0, 1);
            var ex = Assert.Throws<InvalidOperationException>(() => quot = f1 / f2);
            Assert.That(ex.Message, Is.EqualTo("Dividend must not be zero"));
        }
    }
}
