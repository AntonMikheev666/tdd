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
            return new Rectangle(new Point(450, 450), rectangleSize);
        }
    }
}
