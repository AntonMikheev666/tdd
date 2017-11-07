using System;
using TagsCloudVisualization.Implementation;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
//лишние неймспейсы стоит убирать

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class ExtentionsTests
    {
        [Test]
        public void PointExtention_GetLeftTopCorner_ReturnCorrectPoint()
        {
            var rnd = new Random();
            var x = rnd.Next();
            var y = rnd.Next();
            var height = rnd.Next();
            var width = rnd.Next();

            var center = new Point(x, y);

            var leftTopCorner = new Point(x - width / 2, y - height / 2);

            center.GetLeftTopCorner(new Size(width, height)).ShouldBeEquivalentTo(leftTopCorner);
        }

        [Test]
        public void RectangleExtention_GetCenter_ReturnCorrectCenter()
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

        [Test]
        public void RectangleExtention_GetDiagonal_ReturnCorrectDiagonal()
        {
            var rnd = new Random();
            var height = rnd.Next();
            var width = rnd.Next();

            var rect = new Rectangle(0, 0, width, height);

            var actualDiagonal = Math.Sqrt(width * width + height * height);

            rect.GetDiagonal().ShouldBeEquivalentTo(actualDiagonal);
        }
    }
}
