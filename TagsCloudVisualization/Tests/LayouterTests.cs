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
        public void CircularCloudLayouter_FirstRectngle_Centered(int weight, int height)
        {
            layouter.PutNextRectangle(new Size(weight, height)).Location
                .ShouldBeEquivalentTo(new Rectangle(new Point(450, 450), new Size(weight, height)),
                                      opt => opt.ComparingEnumsByValue());
        }
    }
}
