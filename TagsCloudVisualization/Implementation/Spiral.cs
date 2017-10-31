using System;
using System.Drawing;

namespace TagsCloudVisualization.Implementation
{
    public class Spiral
    {
        public readonly Point Center;
        private double currentRadius;
        public readonly double RadiusStep;
        private double currentAngle;
        public readonly double AngleStep;

        public Spiral(Point center, double radiusStep=1, double angleStep=60)
        {
            Center = center;
            RadiusStep = radiusStep;
            AngleStep = angleStep * Math.PI / 360;
        }

        public Point GetNextPoint()
        {
            if (currentRadius == 0)
            {
                currentRadius += RadiusStep;
                return Center;
            }
            var x = (int)Math.Round(currentRadius * Math.Cos(currentAngle));
            var y = (int)Math.Round(currentRadius * Math.Sin(currentAngle));
            currentAngle = (currentAngle + AngleStep) % (Math.PI * 2);
            currentRadius += RadiusStep;
            return new Point(x, y);
        }
    }
}