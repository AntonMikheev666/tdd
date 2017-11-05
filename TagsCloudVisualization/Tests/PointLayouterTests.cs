using System;
using TagsCloudVisualization.Implementation;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
//лишние неймспейсы стоит убирать

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class PointLayouterTests
    {
        private TestSpiralPointLayouter sut;

        [SetUp]
        public void SetUp()
        {
            var center = new Point(500, 500);
            var workingArea=  new Rectangle(500, 500, 1000, 1000);
            sut = new TestSpiralPointLayouter(center, workingArea.GetDiagonal() / 2);
        }

        [TestCase(1, 50, TestName = "PositiveAngleStep")]
        [TestCase(1, -50, TestName = "NegativeAngleStep")]
        public void SpiralPointLayouter_Should_CalculateCorrect(double radiusStep, double angleStep)
        {

            var x = sut.Center.X + (int)Math.Round(2 * radiusStep * Math.Cos(angleStep * Math.PI / 360));
            var y = sut.Center.Y + (int)Math.Round(2 * radiusStep * Math.Sin(angleStep * Math.PI / 360));

            var result = new [] {sut.GetNextPoint(radiusStep, angleStep),
                                 sut.GetNextPoint(radiusStep, 0),
                                 sut.GetNextPoint(radiusStep, angleStep)};
            result.ShouldBeEquivalentTo(
                new [] { sut.Center, new Point(sut.Center.X + (int)radiusStep, sut.Center.Y), new Point(x, y)});
        }

        [Test]
        public void SpirelPointLayouter_NegativeRadiusStepOnFirstCalculation_ShouldThrowException()
        {
            sut.GetNextPoint(1, 50);
            Assert.Throws<ArgumentException>(() => sut.GetNextPoint(-1, 50));
        }

        [Test]
        public void SpirelPointLayouter_TooBigNegativeRadiusStep_ShouldThrowException()
        {
            sut.GetNextPoint(1, 50);

            sut.GetNextPoint(10, 50);
            sut.GetNextPoint(10, 50);

            Assert.Throws<ArgumentException>(() => sut.GetNextPoint(-30, 50));
        }

        [Test]
        public void SpirelPointLayouter_Radius_ChangesCorrectly()
        {
            sut.GetNextPoint(1, 50);

            var rnd = new Random();
            var firstStep = rnd.Next();
            var secondStep = rnd.Next(firstStep);

            sut.TestUpdateAngleAndRadius(firstStep, 0);
            sut.TestUpdateAngleAndRadius(-secondStep, 0);

            sut.CurrentRadius.ShouldBeEquivalentTo(firstStep - secondStep);
        }

        [Test]
        public void SpirelPointLayouter_Angle_ChangesCorrectly()
        {
            sut.GetNextPoint(1, 50);

            var rnd = new Random();
            var firstStep = rnd.Next(int.MinValue, int.MaxValue);
            var secondStep = rnd.Next(int.MinValue, int.MaxValue);

            sut.TestUpdateAngleAndRadius(0, firstStep);
            sut.TestUpdateAngleAndRadius(0, secondStep);

            var actualAngle = firstStep * Math.PI / 360 % (Math.PI * 2);
            actualAngle += secondStep * Math.PI / 360 % (Math.PI * 2);
            Math.Round(sut.CurrentAngle, 5).ShouldBeEquivalentTo(Math.Round(actualAngle, 5));
        }

        [Test]
        public void SpirelPointLayouter_Angle_AngleMoreThen360()
        {
            sut.GetNextPoint(1, 50);

            var rnd = new Random();
            var firstStep = rnd.Next(360, Int32.MaxValue);

            sut.TestUpdateAngleAndRadius(0, firstStep);

            var actualAngle = (firstStep * Math.PI / 360) % (Math.PI * 2);
            Math.Round(sut.CurrentAngle, 5).ShouldBeEquivalentTo(Math.Round(actualAngle, 5));
        }
    }
}
