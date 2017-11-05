using System.Drawing;

namespace TagsCloudVisualization.Implementation
{
    public static class PointExtantion
    {
        public static Point GetLeftTopCorner(this Point center, Size rectSize)
        {
            var x = center.X - rectSize.Width / 2;
            var y = center.Y - rectSize.Height / 2;
            return new Point(x, y);
        }
    }
}