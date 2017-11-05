using System.Drawing;

namespace TagsCloudVisualization.Implementation
{
    public class TestSpiralPointLayouter : SpiralPointLayouter
    {
        public Point Center => center;
        public double CurrentRadius => currentRadius;
        public double CurrentAngle => currentAngle;

        public TestSpiralPointLayouter(Point center, double radiusLimit) : base(center, radiusLimit)
        {
        }

        public void TestUpdateAngleAndRadius(double radiusStep, double angleStep)
        {
            UpdateAngleAndRadius(radiusStep, angleStep);
        }
    }
}