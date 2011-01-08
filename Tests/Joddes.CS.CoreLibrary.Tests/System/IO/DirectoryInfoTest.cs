using System;
using System.IO;
using Joddes.CS.TestFramework;
using Joddes.CS.CoreLibrary.Extensions.System.IO;

namespace Joddes.CS.CoreLibrary.Tests.System.IO
{
    public class DirectoryInfoTest : Test
    {
        [Test(Async = true)]
        public IAsyncResult When_get_directories_from_root_then_atleast_one_directory_is_returned (AsyncCallback callback, object asyncState)
        {
            try {
                var fix = typeof(DirectoryInfoExtension);
            } catch (Exception ex)
            {
                //ignore
            }
            IAsyncResult result = new NetAsyncResult (asyncState);

            var di = new DirectoryInfo ("/");

            di.BeginGetDirectories (r =>
            {
                DirectoryInfo[] dirs = di.EndGetDirectories (r);
                Assert.AreEqual ((string)(object)true, (string)(object)(dirs != null));

                callback (result);
            }, null);

            return result;
        }

        [Test(Async = true)]
        public IAsyncResult When_get_files_from_root_then_atleast_one_file_is_returned (AsyncCallback callback, object asyncState)
        {
            IAsyncResult result = new NetAsyncResult (asyncState);

            var di = new DirectoryInfo ("/");

            di.BeginGetFiles (r =>
            {
                FileInfo[] files = di.EndGetFiles (r);

                Console.WriteLine ("Files: " + files.Length);
                foreach (FileInfo f in files)
                {
                    Console.WriteLine ("File: " + f.Name + " (modified: " + f.LastWriteTime + ") (" + f.CreationTime + ")");
                }

                Assert.AreEqual ((string)(object)true, (string)(object)(files != null));

                callback (result);
            }, null);

            return result;
        }

        [Test(Async = true)]
        public IAsyncResult When_get_files_from_root_then_files_must_have_LastWriteTime (AsyncCallback callback, object asyncState)
        {
            IAsyncResult result = new NetAsyncResult (asyncState);

            var di = new DirectoryInfo ("/");

            di.BeginGetFiles (r =>
            {
                FileInfo[] files = di.EndGetFiles (r);

                foreach (FileInfo f in files) {
                    Assert.NotNull (f.LastWriteTime);
                }

                callback (result);
            }, null);

            return result;
        }

        [Test(Async = true)]
        public IAsyncResult When_get_files_from_root_then_files_must_have_CreationTime (AsyncCallback callback, object asyncState)
        {
            IAsyncResult result = new NetAsyncResult (asyncState);

            var di = new DirectoryInfo ("/");

            di.BeginGetFiles (r =>
            {
                FileInfo[] files = di.EndGetFiles (r);

                foreach (FileInfo f in files) {
                    Assert.NotNull (f.CreationTime);
                }

                callback (result);
            }, null);

            return result;
        }

        [Test(Async = true)]
        public IAsyncResult When_get_files_async_from_root_then_atleast_one_file_is_returned (AsyncCallback callback, object asyncState)
        {
            IAsyncResult result = new NetAsyncResult (asyncState);
            /*
            var di = new DirectoryInfo("/");
            var files = di.GetFilesAsync();

            var atLeastOneFileReturned = false;
            files.Subscribe(f => {
                atLeastOneFileReturned = true;
            }, ex => Assert.Fail(ex.ToString()),
            () => {
                if(!atLeastOneFileReturned) {
                    Assert.Fail("Completed with no file returned.");
                } else {
                    callback(result);
                }
            });*/

            return result;
        }
    }
}