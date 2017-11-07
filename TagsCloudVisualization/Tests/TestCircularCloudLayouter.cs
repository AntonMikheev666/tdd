using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Implementation;

namespace TagsCloudVisualization.Tests
{
    public class TestCircularCloudLayouter : CircularCloudLayouter
    {
        public Point Center => center;
        public Rectangle WorkingArea => workingArea;
        public List<Rectangle> Rectangles => rectangles;
        public TestCircularCloudLayouter(Point center) : base(center)
        {
        }
    }
}