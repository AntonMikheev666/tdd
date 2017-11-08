using System;
using System.Collections;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
//лишние неймспейсы стоит убирать

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class PointComputerTests
    {
        private static TestSpiralPointComputer sut = new TestSpiralPointComputer(new Point(500, 500));

        [SetUp]
        public void SetUp()
        {
            sut = new TestSpiralPointComputer(new Point(500, 500));
        }

        [Test, TestCaseSource(nameof(GetTestCaseData))]
        public Point[] SpiralPointLayouter_Should_CalculateCorrect(double radiusStep, double angleStep)
        {
            var result = new [] {sut.GetNextPoint(0, 0),
                                 sut.GetNextPoint(radiusStep, 0),
                                 sut.GetNextPoint(radiusStep, angleStep)};

            return result;
        }

        [Test]
        public void SpirelPointLayouter_NegativeRadiusStepOnFirstCalculation_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => sut.GetNextPoint(-1, 50));
        }

        [Test]
        public void SpirelPointLayouter_TooBigNegativeRadiusStep_ShouldThrowException()
        {
            sut.GetNextPoint(10, 50);
            sut.GetNextPoint(10, 50);

            Assert.Throws<ArgumentException>(() => sut.GetNextPoint(-30, 50));
        }

        [Test]
        public void SpirelPointLayouter_RandomRadiusSteps_RadiusChangesCorrectly()
        {
            var rnd = new Random();
            var firstStep = rnd.Next();
            var secondStep = rnd.Next(firstStep);

            sut.GetNextPoint(firstStep, 0);
            sut.GetNextPoint(-secondStep, 0);

            sut.CurrentRadius.ShouldBeEquivalentTo(firstStep - secondStep);
        }

        [Test]
        public void SpirelPointLayouter_RandomAngleSteps_AngleChangesCorrectly()
        {
            var rnd = new Random();
            var firstStep = rnd.Next(int.MinValue / 3, int.MaxValue / 3);
            var secondStep = rnd.Next(int.MinValue / 3, int.MaxValue / 3);

            sut.GetNextPoint(0, firstStep);
            sut.GetNextPoint(0, secondStep);

            var actualAngle = firstStep * Math.PI / 360 % (Math.PI * 2);
            actualAngle += secondStep * Math.PI / 360;
            actualAngle %= Math.PI * 2;
            Math.Round(sut.CurrentAngle, 5).ShouldBeEquivalentTo(Math.Round(actualAngle, 5));
        }

        [Test]
        public void SpirelPointLayouter_RandomAngleStep_CutAngleMoreThen360()
        {
            var rnd = new Random();
            var firstStep = rnd.Next(360, Int32.MaxValue);

            sut.GetNextPoint(0, firstStep);

            var actualAngle = (firstStep * Math.PI / 360) % (Math.PI * 2);
            Math.Round(sut.CurrentAngle, 5).ShouldBeEquivalentTo(Math.Round(actualAngle, 5));
        }

        private static IEnumerable GetTestCaseData
        {
            get
            {
                yield return new TestCaseData(1, -50).Returns(GetActualResult(1, -50)).SetName("NegativeAngleStep");
                yield return new TestCaseData(1, 50).Returns(GetActualResult(1, 50)).SetName("PositiveAngleStep");
            }
        }

        private static Point[] GetActualResult(double radiusStep, double angleStep)
        {
            var x = sut.Center.X + (int)Math.Round(2 * radiusStep * Math.Cos(angleStep * Math.PI / 360));
            var y = sut.Center.Y + (int)Math.Round(2 * radiusStep * Math.Sin(angleStep * Math.PI / 360));

            return new[]
            {
                sut.Center,
                new Point(sut.Center.X + (int)Math.Round(radiusStep), sut.Center.Y),
                new Point(x, y)
            };
        }
    }
}
