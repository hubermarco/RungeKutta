
namespace CurveChartImageCreator
{
    internal class CurveTypeID : IDBase
    {
        internal string FrequencyBand { get; private set; }

        internal CurveTypeID(IDBase id)
            : this(id, id, null)
        {
            
        }

        internal CurveTypeID(int id, string name, string freqBand) 
            : base(id, name)
        {
            FrequencyBand = freqBand;
        }

        public override bool Equals(object obj)
        {
            var otherId = obj as CurveTypeID;
            if (otherId == null) return base.Equals(obj);
            return this == (int)otherId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
