
using System;

namespace System.Collections
{
	public interface IEnumerator
	{
		bool MoveNext();
		void Reset();
		object Current { get; }
	}
}