using System;
using System.Drawing;

namespace TagsCloudVisualization.Implementation
{
    public class SpiralPointLayouter
    {
        private readonly Point center;
        public double CurrentRadius { get; private set; }
        private double currentAngle;
        private bool wasUsed;

        public SpiralPointLayouter(Point center)
        {
            this.center = center;
        }
        
        public Point GetNextPoint(double radiusStep, double angleStep)
        {
            if (!wasUsed)
            {
                wasUsed = true;
                return center;
            }

            var angleStepInRadians = angleStep * Math.PI / 360;
            currentAngle = (currentAngle + angleStepInRadians) % (Math.PI * 2);
            CurrentRadius += radiusStep;

            if (CurrentRadius < 0)
                throw new ArgumentException($"Redius can't be negative. " +
                                            $"Current radius: {CurrentRadius}" +
                                            $"Radius step: {radiusStep}.");

            var x = center.X + (int)Math.Round(CurrentRadius * Math.Cos(currentAngle));
            var y = center.Y + (int)Math.Round(CurrentRadius * Math.Sin(currentAngle));
            return new Point(x, y);
        }
    }
}