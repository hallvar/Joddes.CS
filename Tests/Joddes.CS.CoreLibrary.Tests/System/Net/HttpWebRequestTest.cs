using System;
using System.Net;
using System.IO;
using Joddes.CS.TestFramework;

namespace Joddes.CS.CoreLibrary.Tests.System.Net
{
    public class HttpWebRequestTest : Test
    {
        [Test]
        public void When_making_request_for_index_html_then_response_has_content ()
        {
            var req = (HttpWebRequest)WebRequest.Create (new Uri ("index.html"));
            req.BeginGetResponse (r =>
            {
                var response = req.EndGetResponse (r);

                var sr = new StreamReader (response.GetResponseStream ());
                var content = sr.ReadToEnd ();

                Assert.NotNull(content);
            }, null);
        }
    }
}