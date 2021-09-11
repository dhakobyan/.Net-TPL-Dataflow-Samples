using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace System.Threading.Tasks.Dataflow
{
	public static class LinkToExtensions
	{
		public static IDisposable LinkToWithPropagation<T>(this ISourceBlock<T> source, ITargetBlock<T> target)
		{
			return source.LinkTo(target, new DataflowLinkOptions { PropagateCompletion = true });
		}
	}
}
