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
        public void CircularCloudLayouter_FirstRectngle_Centered(int weight, int height)
        {
            var expectedLocation = new Point(layouter.Center.X - weight / 2, 
                                             layouter.Center.Y - height / 2);
            layouter.PutNextRectangle(new Size(weight, height)).Location
                .ShouldBeEquivalentTo(expectedLocation,
                                      opt => opt.ComparingEnumsByValue());
        }
    }
}
