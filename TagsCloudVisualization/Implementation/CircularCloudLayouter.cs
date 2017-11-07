using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Implementation
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        protected List<Rectangle> rectangles;
        private SpiralPointComputer pointComputer;

        public CircularCloudLayouter(Point center)
        {
            rectangles = new List<Rectangle>();
            pointComputer = new SpiralPointComputer(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = GetNextRectangle(rectangleSize);
            while (rectangles.Any(r => r.IntersectsWith(nextRectangle)))
                nextRectangle = GetNextRectangle(rectangleSize);

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }
        
        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            if(rectangles.Count == 0)
                return new Rectangle(pointComputer.GetNextPoint(0, 0).GetLeftTopCorner(rectangleSize),
                                     rectangleSize);
            return new Rectangle(pointComputer.GetNextPoint(0.1, 50).GetLeftTopCorner(rectangleSize), 
                                 rectangleSize);
        }
    }
}