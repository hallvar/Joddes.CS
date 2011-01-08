using System.Collections.Generic;
using Joddes.CS.TestFramework;

namespace Joddes.CS.CoreLibrary.Tests.System.Collections.Generic
{
    public class ListTest : Test
    {
        [Test]
        public void When_adding_an_item_to_an_empty_list_then_item_is_in_first_index ()
        {
            var lst = new List<string> ();

            lst.Add ("test");

            Assert.AreEqual ("test", lst[0]);
        }

        [Test]
        public void When_changing_value_of_first_item_then_first_item_should_be_the_new_value ()
        {
            var lst = new List<string> ();
            lst.Add ("test");
            lst[0] = "test2";

            Assert.AreEqual ("test2", lst[0]);
        }

        [Test]
        public void When_adding_an_item_to_an_empty_list_then_count_is_one ()
        {
            var lst = new List<string> ();
            lst.Add("test");

            Assert.AreEqual ("1", lst.Count.ToString());
        }

        [Test]
        public void When_having_an_empty_list_then_count_is_zero ()
        {
            var lst = new List<string> ();

            Assert.AreEqual ("0", lst.Count.ToString());
        }
    }
}