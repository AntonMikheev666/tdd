using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Implementation
{
    public class CircularCloudLayouter: ICloudLayouter
    {
        private Point center;
        private Rectangle workingArea;
        private SpiralPointLayouter pointLayouter;
        private List<Rectangle> rectangles  = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            SetCenterAndWorkingArea(center);
            pointLayouter = new SpiralPointLayouter(this.center);
        }

        public CircularCloudLayouter(Point center, SpiralPointLayouter spiralPointLayouter)
        {
            SetCenterAndWorkingArea(center);
            this.pointLayouter = spiralPointLayouter;
        }

        private void SetCenterAndWorkingArea(Point center)
        {
            this.center = center;
            var workingAreaSize = new Size(center.X * 2, center.Y * 2);
            workingArea = new Rectangle(new Point(0, 0), workingAreaSize);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height > workingArea.Height || rectangleSize.Width > workingArea.Width)
                throw new ArgumentException($"Incorrect rectangle size: " +
                                            $"Height={rectangleSize.Height} Width={rectangleSize.Width}, " +
                                            $"but maxHeight={workingArea.Height} and " +
                                            $"maxWidth={workingArea.Width}.");

            var nextRectangle = GetNextRectangle(rectangleSize);
            while (rectangles.Any(r => r.IntersectsWith(nextRectangle)) || 
                    !workingArea.Contains(nextRectangle))
                nextRectangle = GetNextRectangle(rectangleSize);

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        //попробовать хранить последнюю точку и искать следующую за О(1)
        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var possibleCenter = pointLayouter.GetNextPoint(0.1, 50);

            if (pointLayouter.CurrentRadius >= workingArea.GetDiagonal() / 2)
                throw new PointSelectionException("Out of free space.");

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
