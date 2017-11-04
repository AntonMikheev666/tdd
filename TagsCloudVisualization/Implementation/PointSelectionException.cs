using System;

namespace TagsCloudVisualization.Implementation
{
    public class PointSelectionException: Exception
    {
        public PointSelectionException(string message): base(message) { }
    }
}