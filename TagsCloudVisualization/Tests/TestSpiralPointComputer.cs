using System.Drawing;
using TagsCloudVisualization.Implementation;

namespace TagsCloudVisualization.Tests
{
    public class TestSpiralPointComputer : SpiralPointComputer
    {
        public Point Center => center;
        public double CurrentRadius => currentRadius;
        public double CurrentAngle => currentAngle;

        public TestSpiralPointComputer(Point center): base(center)
        {
        }
    }
}