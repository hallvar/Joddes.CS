namespace System {

    [Hidden]
    public class Attribute {
        internal Attribute() {
        }
    }

    [Hidden]
    enum AttributeTargets {
        All = 32767,
        Class = 4,
        Struct = 8,
        Enum = 16,                
        Method = 64,
        Interface = 1024, 
        Delegate = 4096
    }

    [Hidden]
    class AttributeUsageAttribute : Attribute {
        public AttributeUsageAttribute(AttributeTargets validOn) { }
        public bool AllowMultiple { get; set; }
    }
}