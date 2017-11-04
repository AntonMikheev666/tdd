using System;
using TagsCloudVisualization.Implementation;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;  
using FluentAssertions;
using NUnit.Framework;
//лишние неймспейсы стоит убирать

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class LayouterTests
    {
        private CircularCloudLayouter SUT; //sut

        [SetUp]
        public void Setup()
        {
            SUT = new CircularCloudLayouter(new Point(500, 500));
        }

        [TestCase(100, 100, TestName = "Square")]
        [TestCase(100, 200, TestName = "VerticalRectangle")]
        [TestCase(200, 100, TestName = "HorizontalRectangle")]
        [TestCase(0, 0, TestName = "Point")]
        [TestCase(1000, 1000, TestName = "BiggestPossiblerectangle")]
        public void CircularCloudLayouter_FirstRectngle_Centered(int weight, int height)
        {
            var actualCenter = new Point(500, 500);
            SUT.PutNextRectangle(new Size(weight, height)).GetCenter()
                .ShouldBeEquivalentTo(actualCenter);
        }

        [TestCase(1001, 1001, TestName = "TooBigRectangle")]
        [TestCase(1, 1001, TestName = "TooHighRectangle")]
        [TestCase(1001, 1, TestName = "TooWideRectangle")]//бесконечное заполнение
        public void CircularCloudLayouter_OutOfBorderRectngle_ShouldThrowException(int weight, int height)
        {
            Assert.Throws<ArgumentException>(() => SUT.PutNextRectangle(new Size(weight, height)));
        }

        [Test]
        public void Spiral_Should_CalculateCorrect()
        {
            int radiusStep = 1, angleStep = 60;
            var spiral = new Spiral(new Point(500, 500), radiusStep, angleStep);

            var x = (int)Math.Round(2 * radiusStep * Math.Cos(angleStep * Math.PI / 360));
            var y = (int)Math.Round(2 * radiusStep * Math.Sin(angleStep * Math.PI / 360));

            var result = new [] {spiral.GetNextPoint(), spiral.GetNextPoint(), spiral.GetNextPoint()};
            result.ShouldBeEquivalentTo(
                new [] {new Point(500, 500), new Point(radiusStep, 0), new Point(x, y)},
                opt => opt.ComparingEnumsByValue());
        }

        [Test]
        public void CircularCloudLayouter_PutNextRectngle_RectanglesDoNotIntersects()
        {
            var size = new Size(50, 50);
            var rectangles = new List<Rectangle>();
            
            for (var i = 0; i < 10; i++)
                rectangles.Add(SUT.PutNextRectangle(size));

            rectangles.
                Select(r => 
                    rectangles.Where(rect => rect != r)
                        .Any(rect => rect.IntersectsWith(r)))
                .ShouldAllBeEquivalentTo(false);
        }

        [Test]
        public void RectangleExtantion_Should_ReturnCorrectCenter()
        {
            var rnd = new Random();
            var x = rnd.Next(int.MinValue, 0);
            var y = rnd.Next(int.MinValue, 0);
            var height = rnd.Next();
            var width = rnd.Next();
            var rectLocation = new Point(x, y);
            var rect = new Rectangle(rectLocation, new Size(width, height));

            var actualCenterX = rectLocation.X + (int)Math.Round(width / 2.0, 0);
            var actualCenterY = rectLocation.Y + (int)Math.Round(height / 2.0, 0);
            var actualCenter = new Point(actualCenterX, actualCenterY);

            rect.GetCenter().ShouldBeEquivalentTo(actualCenter);
        }
    }
}
