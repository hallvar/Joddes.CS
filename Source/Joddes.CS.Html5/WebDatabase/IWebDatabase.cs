using System;
using DOMString=System.String;

namespace Joddes.CS.Html5
{
    public interface IWebDatabase
    {
        Database openDatabase (DOMString name, DOMString version, DOMString displayName, ulong estimatedSize);
        //Database openDatabase(DOMString name, DOMString version, DOMString displayName, ulong estimatedSize, DatabaseCallback creationCallback);
    }
}