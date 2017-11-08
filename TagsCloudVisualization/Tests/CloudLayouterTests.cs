using System;
using System.Collections;
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
        private static Point center = new Point(500, 500);
        private static TestCircularCloudLayouter sut = new TestCircularCloudLayouter(center);

        [SetUp]
        public void SetUp()
        {
            sut = new TestCircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            if(sut.Rectangles.Count == 0)
                return;

            var canvasBitmap = new Bitmap(center.X * 2, center.Y * 2);
            var pen = new Pen(Color.Black);
            var testPictureName = TestContext.CurrentContext.Test.Name + ".png";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, testPictureName);
            var canvas = Graphics.FromImage(canvasBitmap);

            canvas.Clear(Color.White);
            canvas.DrawRectangles(pen, sut.Rectangles.ToArray());
            canvas.Save();

            canvasBitmap.Save(path, ImageFormat.Png);
            Console.WriteLine($"Image was saved in {path}.");
        }

        [TestCase(100, 100, TestName = "Square")]
        [TestCase(100, 200, TestName = "VerticalRectangle")]
        [TestCase(200, 100, TestName = "HorizontalRectangle")]
        [TestCase(0, 0, TestName = "Point")]
        [TestCase(1000, 1000, TestName = "BiggestPossiblerectangle")]
        public void CircularCloudLayouter_FirstRectngle_InCenter(int weight, int height)
        {
            sut.PutNextRectangle(new Size(weight, height))
                .GetCenter()
                .ShouldBeEquivalentTo(center);
        }

        [Test, TestCaseSource(nameof(GetRectangles))]
        public void CircularCloudLayouter_PutNextRectngle_RandomRectanglesDoNotIntersects(List<Rectangle> rectangles)
        {
            rectangles.
                Select(r => 
                    rectangles.Where(rect => rect != r)
                        .Any(rect => rect.IntersectsWith(r)))
                .ShouldAllBeEquivalentTo(false);
        }

        private static IEnumerable GetRectangles
        {
            get
            {
                var rnd = new Random();
                var rectangles = new List<Rectangle>();

                for (var i = 0; i < 500; i++)
                    rectangles.Add(sut.PutNextRectangle(new Size(rnd.Next(5, 25), rnd.Next(5, 25))));

                yield return rectangles;
            }
        }
    }
}
