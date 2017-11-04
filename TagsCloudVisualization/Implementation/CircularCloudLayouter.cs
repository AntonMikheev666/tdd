using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Implementation
{
    public class CircularCloudLayouter //интерфейс
    {
        private Point center { get; }
        private Rectangle workingArea;
        private Spiral spiral;
        private List<Rectangle> rectangles  = new List<Rectangle>();

        public CircularCloudLayouter(Point center, Spiral spiral = null) 
        {
            this.center = center;
            var workingAreaSize = new Size(center.X * 2, center.Y * 2);
            var workingAreaLocation = GetLeftTopCornerLocation(center, workingAreaSize);
            workingArea = new Rectangle(workingAreaLocation, workingAreaSize);
            this.spiral = spiral ?? new Spiral(this.center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height > workingArea.Height || rectangleSize.Width > workingArea.Width)
                throw new ArgumentException($"Incorrect rectangle size: " +
                                            $"Height={rectangleSize.Height} Width={rectangleSize.Width}, " +
                                            $"but maxHeight={workingArea.Height} and " +
                                            $"maxWidth={workingArea.Width}");

            var nextRectangle = GetNextRectangle(rectangleSize);
            while (rectangles.Any(r => r.IntersectsWith(nextRectangle)))
                nextRectangle = GetNextRectangle(rectangleSize);

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }
        //попробовать хранить последнюю точку и искать следующую за О(1)
        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var possibleCenter = spiral.GetNextPoint();
            var leftTopCorner = GetLeftTopCornerLocation(possibleCenter, rectangleSize);
            return new Rectangle(leftTopCorner, rectangleSize);
        }

        private Point GetLeftTopCornerLocation(Point center, Size rectSize)
        {
            var x = center.X - rectSize.Width / 2;
            var y = center.Y - rectSize.Height / 2;
            return new Point(x, y);
        }
    }
}
