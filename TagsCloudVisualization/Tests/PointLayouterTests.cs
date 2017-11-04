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
        [TestCase(1, 50, TestName = "PositiveAngleStep")]
        [TestCase(1, -50, TestName = "NegativeAngleStep")]
        public void SpiralPointLayouter_Should_CalculateCorrect(double radiusStep, double angleStep)
        {
            var center = new Point(500, 500);
            var pointLayouter = new SpiralPointLayouter(center);

            var x = center.X + (int)Math.Round(2 * radiusStep * Math.Cos(angleStep * Math.PI / 360));
            var y = center.Y + (int)Math.Round(2 * radiusStep * Math.Sin(angleStep * Math.PI / 360));

            var result = new [] {pointLayouter.GetNextPoint(radiusStep, angleStep),
                                 pointLayouter.GetNextPoint(radiusStep, 0),
                                 pointLayouter.GetNextPoint(radiusStep, angleStep)};
            result.ShouldBeEquivalentTo(
                new [] {center, new Point(center.X + (int)radiusStep, center.Y), new Point(x, y)});
        }

        [Test]
        public void SpirelPointLayouter_NegativeRadiusStepOnFirstCalculation_ShouldThrowException()
        {
            var pointLayouter = new SpiralPointLayouter(new Point(500, 500));
            pointLayouter.GetNextPoint(1, 50);
            Assert.Throws<ArgumentException>(() => pointLayouter.GetNextPoint(-1, 50));
        }

        [Test]
        public void SpirelPointLayouter_TooBigNegativeRadiusStep_ShouldThrowException()
        {
            var pointLayouter = new SpiralPointLayouter(new Point(500, 500));
            pointLayouter.GetNextPoint(1, 50);

            pointLayouter.GetNextPoint(10, 50);
            pointLayouter.GetNextPoint(10, 50);

            Assert.Throws<ArgumentException>(() => pointLayouter.GetNextPoint(-30, 50));
        }

        [Test]
        public void SpirelPointLayouter_Radius_ChangesCorrectly()
        {
            var pointLayouter = new SpiralPointLayouter(new Point(500, 500));
            pointLayouter.GetNextPoint(1, 50);

            var rnd = new Random();
            var firstStep = rnd.Next();
            var secondStep = rnd.Next(firstStep);

            pointLayouter.GetNextPoint(firstStep, 50);
            pointLayouter.GetNextPoint(-secondStep, 50);

            pointLayouter.CurrentRadius.ShouldBeEquivalentTo(firstStep - secondStep);
        }
    }
}
