using System;

[Hidden, AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public sealed class NativeInlineAttribute : Attribute {
    public NativeInlineAttribute(string format) { }
}
