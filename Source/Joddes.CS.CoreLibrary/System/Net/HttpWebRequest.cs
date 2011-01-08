using System;
using Joddes.CS.Html5;

namespace System.Net
{
    public class HttpWebRequest : WebRequest
    {
        internal Uri uri { get; set; }

        private XmlHttpRequest xhr;

        private bool _endCalled = false;

        private System.IO.MemoryStream _requestStream;

        internal HttpWebRequest ()
        {
        }

        public string Method {
            get; set;
        }

        public string ContentType {
            get {
                return this.Headers["Content-Type"];
            }

            set {
                this.Headers["Content-Type"] = value;
            }
        }

        public long ContentLength {
            get {
                return 0;
            }
        }

        private WebHeaderCollection _headers;
        public WebHeaderCollection Headers {
            get {
                if (_headers == null)
                {
                    _headers = new WebHeaderCollection ();
                }

                return _headers;
            }

            set {
                _headers = value;
            }
        }

        public System.IO.Stream GetRequestStream ()
        {
            if (_requestStream == null)
            {
                _requestStream = new System.IO.MemoryStream ((byte[])(object)"");
            }

            return _requestStream;
        }

        public new IAsyncResult BeginGetResponse (AsyncCallback callback, object state)
        {
            var result = new NetAsyncResult (state);

            xhr = new XmlHttpRequest ();


            xhr.ReadyStateChange += e =>
            {
                if (this.xhr.readyState == this.xhr.DONE) {
                    callback (result);
                }
            };

            if (Method == null)
            {
                Method = "GET";
            }

            xhr.Open (Method, this.uri.AbsoluteUri);

            if (this._headers != null) {
                for (int i = 0; i < this.Headers.Count; i++) {
                    var key = this.Headers.GetKey (i);
                    xhr.SetRequestHeader (key, this.Headers[key]);
                }
            }

             if (this._requestStream != null)
            {
                var sr = new System.IO.StreamReader (this._requestStream);
                var requestDataStr = sr.ReadToEnd();
                xhr.Send(requestDataStr);
            } else {
                xhr.Send ();
            }

            return result;
        }

        public new WebResponse EndGetResponse (IAsyncResult result)
        {
            if (_endCalled) {
                throw new System.Exception ("EndGetResponse already called on this request. It may only be called once.");
            }

            _endCalled = true;
            
            var response = new HttpWebResponse { buffer = (byte[])(object)xhr.ResponseText };
            
            return response;
        }
    }
}
