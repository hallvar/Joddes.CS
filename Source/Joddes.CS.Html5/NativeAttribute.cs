using System;

[Hidden, AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
public sealed class NativeAttribute : Attribute {    
    public NativeAttribute(params string[] lines) { }
}