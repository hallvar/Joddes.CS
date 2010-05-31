namespace System {

    //[Hidden]
    //public class Type { }

    [Hidden]
    public class ValueType { }

    [Hidden]
    public class Enum { }

    //[Hidden]
    //public struct Nullable<T> where T : struct {        
    //    public Nullable(T value) { }

    //    public static implicit operator T?(T value) { return default(T); }
    //    public static explicit operator T(T? value) { return default(T); }

    //    [Obsolete(__CompilerMessages.UNSUPPORTED_METHOD, true)]
    //    public bool HasValue { get { return false; } }

    //    [Obsolete(__CompilerMessages.UNSUPPORTED_METHOD, true)]
    //    public T Value { get { return default(T); } }

    //    [Obsolete(__CompilerMessages.UNSUPPORTED_METHOD, true)]
    //    public T GetValueOrDefault() { return default(T); }

    //    [Obsolete(__CompilerMessages.UNSUPPORTED_METHOD, true)]
    //    public T GetValueOrDefault(T defaultValue) { return default(T); }
    //}

    [Hidden]
    public struct IntPtr { }

    [Hidden]
    public struct UIntPtr { }

    [Hidden]
    public class ParamArrayAttribute { }

    [Hidden]
    public interface IDisposable {
        void Dispose();
    }

    [Hidden]
    public struct Void { }


    
    //[Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    //public struct Byte { }
	
	[Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
	public struct Decimal
	{
	}

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct Int16 { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct Int64 { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct Single { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct Char { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct SByte { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct UInt16 { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct UInt32 { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct UInt64 { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct RuntimeTypeHandle { }

    [Hidden, Obsolete(__CompilerMessages.UNSUPPORTED_CLASS, true)]
    public struct RuntimeFieldHandle { }
}


namespace System.Runtime.InteropServices {
    [Hidden]
    public sealed class OutAttribute {
    }
}

namespace System.Reflection {
    [Hidden]
    public sealed class DefaultMemberAttribute {
        public DefaultMemberAttribute(string memberName) { }
    }
}
/*
namespace System.Collections {

    [Hidden]
    public interface IEnumerable {
        IEnumerator GetEnumerator();
    }

    [Hidden]
    public interface IEnumerator {
        object Current { get; }
        bool MoveNext();
    }

}*/