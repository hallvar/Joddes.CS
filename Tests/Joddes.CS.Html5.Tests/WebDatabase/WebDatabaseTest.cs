using System;
using Joddes.CS.TestFramework;
using Joddes.CS.Html5;

namespace Joddes.CS.Html5.Tests.WebDatabase
{
    public class WebDatabaseTest : Test
    {
        [Test]
        public void When_open_webdatabase_then_database_should_be_non_null()
        {
            var db = Window.Self.openDatabase ("test", "1.0", "Test Db", 100);
            Assert.NotNull(db);
        }
    }
}