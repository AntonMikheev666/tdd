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
        [TestCase(100, 100, TestName = "Square")]
        [TestCase(100, 200, TestName = "VerticalRectangle")]
        [TestCase(200, 100, TestName = "HorizontalRectangle")]
        [TestCase(0, 0, TestName = "Point")]
        [TestCase(1000, 1000, TestName = "BiggestPossiblerectangle")]
        public void CircularCloudLayouter_FirstRectngle_Centered(int weight, int height)
        {
            var center = new Point(500, 500);
            var sut = new CircularCloudLayouter(center);
            sut.PutNextRectangle(new Size(weight, height))
                .GetCenter()
                .ShouldBeEquivalentTo(center);
        }

        [TestCase(1001, 1001, TestName = "TooBigRectangle")]
        [TestCase(1, 1001, TestName = "TooHighRectangle")]
        [TestCase(1001, 1, TestName = "TooWideRectangle")]//бесконечное заполнение
        public void CircularCloudLayouter_OutOfBorderRectngle_ShouldThrowException(int weight, int height)
        {
            var center = new Point(500, 500);
            var sut = new CircularCloudLayouter(center);
            Assert.Throws<ArgumentException>(() => sut.PutNextRectangle(new Size(weight, height)));
        }

        [TestCase(1, 50, TestName = "PositiveAngleStep")]
        [TestCase(1, -50, TestName = "NegativeAngleStep")]
        public void SpiralPointLayouter_Should_CalculateCorrect(double radiusStep, double angleStep)
        {
            var pointLayouter = new SpiralPointLayouter(new Point(500, 500));

            var x = (int)Math.Round(2 * radiusStep * Math.Cos(2*angleStep * Math.PI / 360));
            var y = (int)Math.Round(2 * radiusStep * Math.Sin(2*angleStep * Math.PI / 360));

            var result = new [] {pointLayouter.GetNextPoint(radiusStep, angleStep),
                                 pointLayouter.GetNextPoint(radiusStep, angleStep),
                                 pointLayouter.GetNextPoint(radiusStep, angleStep)};
            result.ShouldBeEquivalentTo(
                new [] {new Point(500, 500), new Point((int)radiusStep, 0), new Point(x, y)},
                opt => opt.ComparingEnumsByValue());
        }

        [Test]
        public void SpirelPointLayouter_NegativeRadiusStepOnFirstCalculation_ShouldThrowException()
        {
            var pointLayouter = new SpiralPointLayouter(new Point(500, 500));
            pointLayouter.GetNextPoint(1, 50);
            Assert.Throws<ArgumentException>(() => pointLayouter.GetNextPoint(-1, 50));
        }

        [Test]
        public void SpirelPointLayouter_TooBigRadiusStep_ShouldThrowException()
        {
            var pointLayouter = new SpiralPointLayouter(new Point(500, 500));
            pointLayouter.GetNextPoint(1, 50);

            pointLayouter.GetNextPoint(10, 50);
            pointLayouter.GetNextPoint(10, 50);

            Assert.Throws<ArgumentException>(() => pointLayouter.GetNextPoint(-30, 50));
        }

        [Test]
        public void SpirelPointLayouter_TooBigRadiusStep_()
        {
            var pointLayouter = new SpiralPointLayouter(new Point(500, 500));
            pointLayouter.GetNextPoint(1, 50);

            pointLayouter.GetNextPoint(10, 50);
            pointLayouter.GetNextPoint(10, 50);

            Assert.Throws<ArgumentException>(() => pointLayouter.GetNextPoint(-30, 50));
        }

        [Test]
        public void CircularCloudLayouter_PutNextRectngle_RectanglesDoNotIntersects()
        {
            var center = new Point(500, 500);
            var sut = new CircularCloudLayouter(center);

            var size = new Size(50, 50);
            var rectangles = new List<Rectangle>();
            
            for (var i = 0; i < 10; i++)
                rectangles.Add(sut.PutNextRectangle(size));

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
