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

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectX = Center.X - rectangleSize.Width / 2;
            var rectY = Center.Y - rectangleSize.Height / 2;
            return new Rectangle(new Point(rectX, rectY), rectangleSize);
        }
    }
}
