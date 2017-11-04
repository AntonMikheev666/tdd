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
    class ExtantionTests
    {
        [Test]
        public void RectangleGetCenter_Should_ReturnCorrectCenter()
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
