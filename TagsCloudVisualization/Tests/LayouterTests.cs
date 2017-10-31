using System;
using TagsCloudVisualization.Implementation;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class LayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void Setup()
        {
            layouter = new CircularCloudLayouter(new Point(500, 500));
        }

        [TestCase(100, 100, TestName = "Square")]
        [TestCase(100, 200, TestName = "VerticalRectangle")]
        [TestCase(200, 100, TestName = "HorizontalRectangle")]
        [TestCase(0, 0, TestName = "Point")]
        [TestCase(1000, 1000, TestName = "BiggestPossiblerectangle")]
        public void CircularCloudLayouter_FirstRectngle_Centered(int weight, int height)
        {
            var expectedLocation = new Point(layouter.Center.X - weight / 2, 
                                             layouter.Center.Y - height / 2);
            layouter.PutNextRectangle(new Size(weight, height)).Location
                .ShouldBeEquivalentTo(expectedLocation,
                                      opt => opt.ComparingEnumsByValue());
        }

        [TestCase(1001, 1001, TestName = "TooBigRectangle")]
        [TestCase(1, 1001, TestName = "TooHighRectangle")]
        [TestCase(1001, 1, TestName = "TooWideRectangle")]
        public void CircularCloudLayouter_OutOfBorderRectngle_ShouldThrowException(int weight, int height)
        {
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(weight, height)));
        }

        [Test]
        public void Spiral_Should_CalculateCorrect()
        {
            int radiusStep = 1, angleStep = 60;
            var spiral = new Spiral(layouter.Center, radiusStep, angleStep);

            var x = (int)Math.Round(2 * radiusStep * Math.Cos(angleStep * Math.PI / 360));
            var y = (int)Math.Round(2 * radiusStep * Math.Sin(angleStep * Math.PI / 360));

            var result = new [] {spiral.GetNextPoint(), spiral.GetNextPoint(), spiral.GetNextPoint()};
            result.ShouldBeEquivalentTo(
                new [] {layouter.Center, new Point(radiusStep, 0), new Point(x, y)},
                opt => opt.ComparingEnumsByValue());
        }
    }
}
