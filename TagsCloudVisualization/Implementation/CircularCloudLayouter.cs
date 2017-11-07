using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Implementation
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        protected List<Rectangle> rectangles;
        private SpiralPointLayouter pointLayouter;

        public CircularCloudLayouter(Point center)
        {
            rectangles = new List<Rectangle>();
            pointLayouter = new SpiralPointLayouter(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = GetNextRectangle(rectangleSize);
            while (rectangles.Any(r => r.IntersectsWith(nextRectangle)))
                nextRectangle = GetNextRectangle(rectangleSize);

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        //попробовать хранить последнюю точку и искать следующую за О(1)
        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var possibleCenter = pointLayouter.GetNextPoint(0.1, 50);

            var leftTopCorner = possibleCenter.GetLeftTopCorner(rectangleSize);
            return new Rectangle(leftTopCorner, rectangleSize);
        }
    }
}