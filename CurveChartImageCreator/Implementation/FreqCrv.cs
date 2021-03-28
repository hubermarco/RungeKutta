using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;


namespace CurveChartImageCreator
{
    internal class FreqCrv : Collection<FreqPt>, IFrequencyCurve
    {
        internal FreqCrv(IDBase curveType)

        {
        }

        internal FreqCrv(String curveName)
        {
            CurveType = null;
            CurveName = curveName;
        }

        internal IDBase CurveType { get; set; }

        IFrequencyPoint IFrequencyCurve.this[ int index ]
        {
            get { return this[index]; }
        }

        void IFrequencyCurve.SetPoint( int index, double x, double y )
        {
            this[ index ] = new FreqPt( x, y );
        }

        internal String CurveName { get; private set; }

        IDBase IFrequencyCurve.CurveType => throw new NotImplementedException();

        internal FreqCrv GetCopy()
        {
            var freqCurve = CurveType != null ? new FreqCrv(CurveType ) : new FreqCrv(CurveName);

            foreach (var pt in Items)
            {
                freqCurve.Add( new FreqPt(pt.X, pt.Y ) );
            }

            return freqCurve;
        }

        internal MemoryStream Serialize()
        {
            var stream = new MemoryStream();
            foreach (var freqPt in Items)
            {
                freqPt.Serialize( stream );
            }
            stream.Seek( 0, 0 );
            return stream;
        }

        public void DeSerialize(MemoryStream stream)
        {
            Clear();
            stream.Seek(0, 0);
            while (stream.Position < stream.Length)
            {
                Add(FreqPt.DeSerialize(stream));
            }
        }

        #region Implementation of IEnumerable<out IFrequencyPoint>

        IEnumerator<IFrequencyPoint> IEnumerable<IFrequencyPoint>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private const char Delimeter = ' ';

       

        public MemoryStream ToStream()
        {
            var valueStream = new MemoryStream();
            foreach (var freqPt in Items)
            {
                valueStream.Write(BitConverter.GetBytes(freqPt.Y), 0, sizeof(double));
                if (freqPt.Z.HasValue)
                    valueStream.Write(BitConverter.GetBytes(freqPt.Z.Value), 0, sizeof(double));
            }
            return Compress(valueStream);
        }

      
        private static double? GetValue(MemoryStream valueStream)
        {
            const int valueSize = sizeof(double);
            var buffer = new byte[valueSize];
            if (valueStream.Read(buffer, 0, valueSize) < valueSize)
                return null;
            return BitConverter.ToDouble(buffer, 0);
        }

        private static MemoryStream Compress(MemoryStream valueStream)
        {
            var stream = new MemoryStream();
            valueStream.Position = 0;
            using (Stream fileStream = valueStream,
                deflateStream = new DeflateStream(stream, CompressionMode.Compress, true)
            )
            {
                fileStream.CopyTo(deflateStream);
            }

            return stream;
        }

        private static MemoryStream Decompress(MemoryStream stream)
        {
            try
            {
                stream.Position = 0;
                var decompressedStream = new MemoryStream();
                using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
                {
                    deflateStream.CopyTo(decompressedStream);
                    decompressedStream.Position = 0;
                    return decompressedStream;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}