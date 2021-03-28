
namespace CurveChartImageCreator
{
    internal interface IFrequencyPoint
    {
        double X { get; }
        double Y { get; set; }
        double? Z { get; }
    }
}