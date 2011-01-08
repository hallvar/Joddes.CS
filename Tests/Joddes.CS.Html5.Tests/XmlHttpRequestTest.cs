using System;
using Joddes.CS.TestFramework;
using Joddes.CS.Html5;

namespace Joddes.CS.Html5.Tests
{
    public class XmlHttpRequestTest : Test
    {
        private XmlHttpRequest xhr;

        public override void Setup ()
        {
            xhr = new XmlHttpRequest();
        }

        public override void TearDown ()
        {
            xhr = null;
        }

        [Test(Async = true)]
        public IAsyncResult When_url_is_malformed_then_send_fails (AsyncCallback callback, object asyncState)
        {
            var result = new NetAsyncResult (asyncState);

            xhr.Open ("GET", "malformed://321.321.321.321/");
            xhr.ReadyStateChange += e =>
            {
                Assert.AreEqual ((string)(object)xhr.DONE, (string)(object)xhr.readyState);
                Assert.AreEqual ("", xhr.ResponseText);
                Assert.AreEqual (null, (string)(object)xhr.ResponseXML);
                callback (result);
            };

            bool fail = true;
            try {
                xhr.Send ();
                fail = false;
            } catch (Exception ex)
            {
            }

            if (!fail)
            {
                Assert.Fail ("Send didn't fail");
            }

            return result;
        }

        [Test(Async = true)]
        public IAsyncResult When_url_is_to_this_file_then_responseText_is_not_empty (AsyncCallback callback, object asyncState)
        {
            var result = new NetAsyncResult (asyncState);

            xhr.Open("GET", "bin/js/Joddes/CS/Html5/Tests/XmlHttpRequestTest.js");

            xhr.ReadyStateChange += e => {
                if(xhr.readyState == xhr.DONE) {
                    Assert.AreEqual((string)(object)true, (string)(object)(xhr.ResponseText.Length > 0));
                    callback(result);
                }
            };

            xhr.Send();

            return result;
        }

        [Test(Async = true)]
        public IAsyncResult When_WebDAV_PROPFIND_request_then_response_is_multistatus (AsyncCallback callback, object asyncState)
        {
            var result = new NetAsyncResult (asyncState);

            xhr.Open("PROPFIND", "/");

            xhr.SetRequestHeader ("Content-Type", "text/xml; charset=UTF-8");
            xhr.SetRequestHeader ("Depth", "1");

            xhr.ReadyStateChange += e =>
            {
                if (xhr.readyState == xhr.DONE) {
                    Assert.AreEqual("207", (string)(object)xhr.Status);
                    var elements = xhr.ResponseXML.GetElementsByTagName("multistatus");
                    Assert.AreEqual("1", (string)(object)elements.Length);

                    callback(result);
                }
            };

            string webdavCmd = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                    "<D:propfind xmlns:D=\"DAV:\">" +
                    "<D:allprop />" +
                    "</D:propfind>";

            xhr.Send (webdavCmd);

            return result;
        }
    }
}