
using System.Collections.Generic;

namespace CurveChartImageCreator
{
    internal interface IFrequencyCurve : IEnumerable<IFrequencyPoint>
    {
        IDBase CurveType { get; }
        IFrequencyPoint this[int index] { get; }
        void SetPoint( int index, double x, double y );
    }
}