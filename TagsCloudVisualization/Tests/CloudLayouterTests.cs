using System;
using TagsCloudVisualization.Implementation;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;  
using FluentAssertions;
using NUnit.Framework;
//лишние неймспейсы стоит убирать

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class CloudLayouterTests
    {
        private CircularCloudLayouter sut;

        [SetUp]
        public void SetUp()
        {
            sut = new CircularCloudLayouter(new Point(500, 500));
        }

        [TearDown]
        public void TearDown()
        {
            var canvasBitmap = new Bitmap(sut.GetWorkingArea.Width, sut.GetWorkingArea.Height);
            var pen = new Pen(Color.Black);
            var testPictureName = TestContext.CurrentContext.Test.Name + ".png";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, testPictureName);
            var canvas = Graphics.FromImage(canvasBitmap);

            canvas.Clear(Color.White);
            canvas.DrawRectangles(pen, sut.Rectangles.ToArray());
            canvas.Save();

            canvasBitmap.Save(path, ImageFormat.Png);
        }

        [TestCase(100, 100, TestName = "Square")]
        [TestCase(100, 200, TestName = "VerticalRectangle")]
        [TestCase(200, 100, TestName = "HorizontalRectangle")]
        [TestCase(0, 0, TestName = "Point")]
        [TestCase(1000, 1000, TestName = "BiggestPossiblerectangle")]
        public void CircularCloudLayouter_FirstRectngle_Centered(int weight, int height)
        {
            sut.PutNextRectangle(new Size(weight, height))
                .GetCenter()
                .ShouldBeEquivalentTo(new Point(500, 500));
        }

        [TestCase(1001, 1001, TestName = "TooBigRectangle")]
        [TestCase(1, 1001, TestName = "TooHighRectangle")]
        [TestCase(1001, 1, TestName = "TooWideRectangle")]
        public void CircularCloudLayouter_IncorrectSizeRectngle_ShouldThrowException(int weight, int height)
        {
            Assert.Throws<ArgumentException>(() => sut.PutNextRectangle(new Size(weight, height)));
        }

        [Test]
        public void CircularCloudLayouter_OutOfBorderRectngle_ShouldThrowException()
        {
            sut.PutNextRectangle(new Size(1000, 1000));

            Assert.Throws<PointSelectionException>(() => sut.PutNextRectangle(new Size(1, 1)));
        }

        [Test]
        public void CircularCloudLayouter_PutNextRectngle_RectanglesDoNotIntersects()
        {
            var size = new Size(25, 25);
            var rectangles = new List<Rectangle>();
            
            for (var i = 0; i < 100; i++)
                rectangles.Add(sut.PutNextRectangle(size));

            rectangles.
                Select(r => 
                    rectangles.Where(rect => rect != r)
                        .Any(rect => rect.IntersectsWith(r)))
                .ShouldAllBeEquivalentTo(false);
        }
    }
}
