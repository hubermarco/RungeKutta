using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CurveChartImageCreator
{
    internal class FreqPt : IFrequencyPoint
    {
        internal FreqPt( double x, double y)
        {
            X = x;
            Y = y;
        }

        internal FreqPt(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        internal double Y { get; set; }
        internal double X { get; }
        internal double? Z { get; }

        double IFrequencyPoint.X => throw new NotImplementedException();

        double IFrequencyPoint.Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        double? IFrequencyPoint.Z => throw new NotImplementedException();

        internal void Serialize(MemoryStream stream)
        {
            stream.Write(BitConverter.GetBytes(X), 0, sizeof(double));
            stream.Write(BitConverter.GetBytes(Y), 0, sizeof(double));            
        }

        internal static FreqPt DeSerialize(MemoryStream stream)
        {
            return new FreqPt(GetValue(stream), GetValue(stream));            
        }

        private static double GetValue(Stream stream)
        {
            var bytes = new byte[sizeof(double)];
            var length = stream.Read(bytes, 0, sizeof(double));
            if (length == sizeof(double))
                return BitConverter.ToDouble(bytes, 0);
            Trace.Write(String.Format("GetValue reads {0} instead of {1}", length, sizeof(double)));
            throw new InvalidDataException();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "X:{0}, Y:{1}", X, Y);
        }
    }
}