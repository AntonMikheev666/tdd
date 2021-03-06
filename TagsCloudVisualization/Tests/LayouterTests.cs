﻿using System;
using TagsCloudVisualization.Implementation;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;  
using FluentAssertions;
using NUnit.Framework;
//лишние неймспейсы стоит убирать

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class LayouterTests
    {
        private CircularCloudLayouter layouter; //sut

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
        [TestCase(1001, 1, TestName = "TooWideRectangle")]//бесконечное заполнение
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

        [Test]
        public void CircularCloudLayouter_PutNextRectngle_RectanglesDoNotIntersects()
        {
            var size = new Size(50, 50);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 10; i++)
                rectangles.Add(layouter.PutNextRectangle(size));

            var intersects = false; //LINQ
            foreach (var rectangle in rectangles)
                intersects = rectangles
                    .Where(r => r != rectangle)
                    .Any(r => r.IntersectsWith(rectangle));
            
            intersects.Should().BeFalse();
        }
    }
}
