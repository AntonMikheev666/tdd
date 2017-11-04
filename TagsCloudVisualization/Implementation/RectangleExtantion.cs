using System;
using System.Drawing;

namespace TagsCloudVisualization.Implementation
{
    public static class RectangleExtantion
    {
        public static Point GetCenter(this Rectangle rect)
        {
            var centerX = rect.Location.X + (int)Math.Round(rect.Width / 2.0, 0);
            var centerY = rect.Location.Y + (int)Math.Round(rect.Height / 2.0, 0);
            return new Point(centerX, centerY);
        }
    }
}