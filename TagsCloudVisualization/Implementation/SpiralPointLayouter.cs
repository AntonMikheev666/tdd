using System;
using System.Drawing;

namespace TagsCloudVisualization.Implementation
{
    public class SpiralPointLayouter
    {
        private readonly Point center;
        private double currentRadius;
        private double currentAngle;
        private bool wasUsed;

        public SpiralPointLayouter(Point center)
        {
            this.center = center;
        }

        //динамический шаг + тесты на парматеры
        public Point GetNextPoint(double radiusStep, double angleStep)
        {
            if (!wasUsed)
            {
                wasUsed = true;
                return center;
            }

            var angleStepInRadians = angleStep * Math.PI / 360;
            currentAngle = (currentAngle + angleStepInRadians) % (Math.PI * 2);
            currentRadius += radiusStep;

            if (currentRadius < 0)
                throw new ArgumentException($"Redius can't be negative. " +
                                            $"Current radius: {currentRadius}" +
                                            $"Radius step: {radiusStep}");

            var x = (int)Math.Round(currentRadius * Math.Cos(currentAngle));
            var y = (int)Math.Round(currentRadius * Math.Sin(currentAngle));
            return new Point(x, y);
        }
    }
}