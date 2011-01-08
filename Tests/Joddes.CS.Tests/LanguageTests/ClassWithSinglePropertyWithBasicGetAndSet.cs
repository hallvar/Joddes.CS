using System;
namespace Joddes.CS.Tests
{
    public class ClassWithSinglePropertyWithBasicGetAndSet
    {
        private string singlePropertyWithBasicGetAndSet;
        public string SinglePropertyWithBasicGetAndSet {
            get {
                return singlePropertyWithBasicGetAndSet;
            }

            set {
                singlePropertyWithBasicGetAndSet = value;
            }
        }
    }
}