using System;
using System.Collections.Generic;
using System.IO;
using Joddes.CS.Html5;

namespace Joddes.CS.CoreLibrary.Extensions.System.IO
{
    public static class DirectoryInfoExtension
    {
        public static IAsyncResult BeginGetDirectories (this DirectoryInfo di, AsyncCallback callback, object asyncState)
        {
            var result = new IOAsyncResult (asyncState);

            var xhr = new Joddes.CS.Html5.XmlHttpRequest();

            xhr.Open ("PROPFIND", di.FullName);

            xhr.SetRequestHeader ("Content-Type", "text/xml; charset=UTF-8");
            xhr.SetRequestHeader ("Depth", "1");
            
            xhr.ReadyStateChange += e =>
            {
                if (xhr.readyState == xhr.DONE) {

                    if(xhr.Status != 404) {
                        var elements = xhr.ResponseXML.GetElementsByTagName ("href");
                        result.Data = elements;
                    }

                    callback (result);
                }
            };

            string webdavCmd = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + "<D:propfind xmlns:D=\"DAV:\">" + "<D:allprop />" + "</D:propfind>";

            xhr.Send (webdavCmd);

            return result;
        }

        public static DirectoryInfo[] EndGetDirectories (this DirectoryInfo di, IAsyncResult result)
        {
            var directories = new List<DirectoryInfo> ();
            var elements = (HTMLElement[])((IOAsyncResult)result).Data;

            if (elements != null) {
                foreach (HTMLElement e in elements)
                {
                    var tags = e.getElementsByTagName ("getcontenttype");
                    if(tags.Length > 0) {
                        Node item = tags[0];
                        var type = item.TextContent;
                        if(type == "httpd/unix-directory") {
                            var hrefs = e.getElementsByTagName ("href");
                            Node hrefNode = hrefs[0];
                            var href = hrefNode.TextContent;
                            var d = new DirectoryInfo (href);
                            directories.Add (d);
                        }
                    }
                }
            }

            return directories.ToArray ();
        }

        public static IAsyncResult BeginGetFiles (this DirectoryInfo di, AsyncCallback callback, object asyncState)
        {
            var result = new IOAsyncResult (asyncState);
            
            var xhr = new Joddes.CS.Html5.XmlHttpRequest ();
            
            xhr.Open ("PROPFIND", di.FullName);
            
            xhr.SetRequestHeader ("Content-Type", "text/xml; charset=UTF-8");
            xhr.SetRequestHeader ("Depth", "1");
            
            xhr.ReadyStateChange += e =>
            {
                if (xhr.readyState == xhr.DONE) {
                    
                    if (xhr.Status != 404) {
                        var elements = xhr.ResponseXML.GetElementsByTagName ("response");
                        result.Data = elements;
                    }
                    
                    callback (result);
                }
            };
            
            string webdavCmd = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
                    + "<D:propfind xmlns:D=\"DAV:\">"
                        + "<D:allprop />"
                    + "</D:propfind>";

            xhr.Send (webdavCmd);
            
            return result;
        }

        public static FileInfo[] EndGetFiles (this DirectoryInfo di, IAsyncResult result)
        {
            var files = new List<FileInfo> ();
            var elements = (HTMLElement[])((IOAsyncResult)result).Data;

            if (elements != null) {
                foreach (HTMLElement e in elements) {
                    var tags = e.getElementsByTagName ("getcontenttype");
                    if(tags.Length > 0) {
                        Node item = tags[0];
                        var type = item.TextContent;
                        if(type == "httpd/unix-directory") {
                            continue;
                        }
                    }
                    var hrefs = e.getElementsByTagName("href");
                    Node hrefNode = hrefs[0];
                    var href = hrefNode.TextContent;
                    var d = new FileInfo (href);

                    var cdates = e.getElementsByTagName("creationdate");
                    Node cdateNode = cdates[0];
                    var cdate = cdateNode.TextContent;

                    d.CreationTime = DateTime.Parse(cdate);

                    var mdates = e.getElementsByTagName ("getlastmodified");
                    Node mdateNode = mdates[0];
                    var mdate = mdateNode.TextContent;

                    d.LastWriteTime = DateTime.Parse(mdate);

                    files.Add (d);
                }
            }

            return files.ToArray ();
        }

        public static IObservable<FileInfo> GetFilesAsync (this DirectoryInfo di)
        {
            var obs = new IOObservable<FileInfo> ();

            var xhr = new Joddes.CS.Html5.XmlHttpRequest ();
            
            xhr.Open ("PROPFIND", di.FullName);
            
            xhr.SetRequestHeader ("Content-Type", "text/xml; charset=UTF-8");
            xhr.SetRequestHeader ("Depth", "1");
            
            xhr.ReadyStateChange += ev =>
            {
                if (xhr.readyState == xhr.DONE) {
                    
                    if (xhr.Status != 404) {
                        var elements = xhr.ResponseXML.GetElementsByTagName ("response");

                        if (elements != null) {
                            foreach (HTMLElement e in elements) {
                                var tags = e.getElementsByTagName ("getcontenttype");
                                if (tags.Length > 0) {
                                    Node item = tags[0];
                                    var type = item.TextContent;
                                    if (type == "httpd/unix-directory") {
                                        continue;
                                    }
                                }
                                var hrefs = e.getElementsByTagName ("href");
                                Node hrefNode = hrefs[0];
                                var href = hrefNode.TextContent;
                                var d = new FileInfo (href);
                                
                                var cdates = e.getElementsByTagName ("creationdate");
                                Node cdateNode = cdates[0];
                                var cdate = cdateNode.TextContent;
                                
                                d.CreationTime = DateTime.Parse (cdate);
                                
                                var mdates = e.getElementsByTagName ("getlastmodified");
                                Node mdateNode = mdates[0];
                                var mdate = mdateNode.TextContent;
                                
                                d.LastWriteTime = DateTime.Parse (mdate);
                                
                                obs.OnNext(d);
                            }
                            obs.OnCompleted();
                        }
                    }
                    
                    //callback (result);
                }
            };
            
            string webdavCmd = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + "<D:propfind xmlns:D=\"DAV:\">" + "<D:allprop />" + "</D:propfind>";
            
            xhr.Send (webdavCmd);

            return obs;
        }
    }
}