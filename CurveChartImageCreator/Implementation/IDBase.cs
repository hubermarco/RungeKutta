
namespace CurveChartImageCreator
{
    using System;
    using System.Linq;

	internal abstract class IDBase
	{
		private readonly int _id;
		private readonly string _name;

		protected IDBase( int id, string name )
		{
			_id = id;
			_name = name;
		}

		public static implicit operator int( IDBase controlId )
		{
			return controlId != null ? controlId.ToInt() : 0;
		}

		public static implicit operator string (IDBase controlName)
        {
            return controlName !=null ? controlName.ToString() : string.Empty;
        }

		internal virtual int ToInt()
	    {
            return _id;
	    }

		public override string ToString()
		{
			return _name;
		}

		public override bool Equals( object obj )
		{
			var otherId = obj as IDBase;
			if (otherId == null) return false;
            if (!GetType().IsInstanceOfType(otherId) &&
                !otherId.GetType().IsInstanceOfType(this)) return false;
			return Equals( ToInt(), otherId.ToInt() );
		}

		public override int GetHashCode()
		{
			return ToInt().GetHashCode();
		}

        public static bool operator==( IDBase firstID, IDBase secondID )
	    {
	        return Equals( firstID, secondID );
	    }

	    public static bool operator !=( IDBase firstID, IDBase secondID )
	    {
	        return !( firstID == secondID );
	    }

		internal static TId Parse<TColl, TId>( string key ) where TId : IDBase
	    {
			return Parse<TId>( typeof (TColl), key );
		}

		internal static TId Parse<TId>(Type collType, string key) where TId : IDBase
		{
            if( string.IsNullOrEmpty( key ) ) return null;

			var result = collType.GetFields()
				.Where( field => field.FieldType == typeof(TId) && field.Name.ToUpperInvariant() == key.ToUpperInvariant() )
				.Select(field => (TId)field.GetValue(null) )
				.FirstOrDefault();

			return result;
		}

		internal static TId Parse<TId>( Type collType, int controlNumber )
	    {
	        var result = collType.GetFields()
                        .Where(field => field.FieldType == typeof(TId) && ((IDBase)field.GetValue(null))._id == controlNumber)
	                    .Select( field => (TId)field.GetValue( null ) )
	                    .FirstOrDefault();

	        return result;
	    }

		internal static TId ParseValue<TId>(Type collType, string controlValue)
        {
            var result = collType.GetFields()
                        .Where(field => field.FieldType == typeof(TId) && ((IDBase)field.GetValue(null))._name == controlValue)
                        .Select(field => (TId)field.GetValue(null))
                        .FirstOrDefault();

            return result;
        }

	}
}