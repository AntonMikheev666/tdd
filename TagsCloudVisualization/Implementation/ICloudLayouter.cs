using System.Drawing;

namespace TagsCloudVisualization.Implementation
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}