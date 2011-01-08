using System;
using Joddes.CS.TestFramework;
using Joddes.CS.Tests.LanguageTests;

namespace Joddes.CS.Tests
{
    public class LanguageTest : Test
    {
        [Test]
        public void Empty_class ()
        {
            var t = new EmptyClass ();

            Assert.NotNull (t);
        }

        [Test]
        public void Empty_class_with_default_constructor ()
        {
            var t = new EmptyClassWithDefaultConstructor ();

            Assert.NotNull (t);
        }

        [Test]
        public void Class_with_single_empty_method ()
        {
            var t = new ClassWithSingleEmptyMethod ();

            t.SingleEmptyMethod ();

            var cameToHere = "yes";

            Assert.AreEqual ("yes", cameToHere);
        }

        [Test]
        public void Class_with_single_property_with_default_get_and_set ()
        {
            var t = new ClassWithSinglePropertyWithDefaultGetAndSet ();

            t.SinglePropertyWithDefaultGetAndSet = "test";

            Assert.AreEqual ("test", t.SinglePropertyWithDefaultGetAndSet);
        }

        [Test]
        public void Class_with_single_property_with_basic_get_and_set ()
        {
            var t = new ClassWithSinglePropertyWithBasicGetAndSet ();

            t.SinglePropertyWithBasicGetAndSet = "test";

            Assert.AreEqual ("test", t.SinglePropertyWithBasicGetAndSet);
        }

        [Test]
        public void Class_with_methods_properties_fields_events_and_delegates ()
        {
            var t = new ClassWithMethodsPropertiesFieldsEventsAndDelegates ();

            t.MyEvent += (str) => {
                t.MyProperty = str;
            };

            var myStr = t.MyMethodCausesMyEventWithString("test");

            Assert.AreEqual("test", myStr);
            Assert.AreEqual("test", t.MyProperty);
        }
    }
}