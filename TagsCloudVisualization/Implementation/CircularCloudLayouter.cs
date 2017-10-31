using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Implementation
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        private Spiral spiral;
        private List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center, Spiral spiral = null)
        {
            Center = center;
            this.spiral = spiral ?? new Spiral(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height > Center.X * 2 || rectangleSize.Width > Center.Y * 2)
                throw new ArgumentException("Inappropriate rectangle size");

            var result = GetNextPossibleRectangle(rectangleSize);
            while (rectangles.Any(r => r.IntersectsWith(result)))
                result = GetNextPossibleRectangle(rectangleSize);
            rectangles.Add(result);
            return result;
        }

        private Point GetNextPossibleRectangleCenter()
        {
            var result = spiral.GetNextPoint();
            while (rectangles.Any(r => r.Contains(result)))
                result = spiral.GetNextPoint();
            return result;
        }

        private Rectangle GetNextPossibleRectangle(Size rectangleSize)
        {
            var possibleCenter = GetNextPossibleRectangleCenter();
            var rectX = possibleCenter.X - rectangleSize.Width / 2;
            var rectY = possibleCenter.Y - rectangleSize.Height / 2;
            return new Rectangle(new Point(rectX, rectY), rectangleSize);
        }
    }
}
