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
    class CloudLayouterTests
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
    }
}
