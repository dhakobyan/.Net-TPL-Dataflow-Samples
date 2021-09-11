using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class EncapsulatedCustomBlockSample
	{
		public static async Task Start()
		{
			var inputBlock = new TransformBlock<int, int>(x =>
			{
				if (x == 2)
					throw new Exception("Something went wrong!");

				return x;
			});
			var increasingBlock = CreateFilteringBlock<int>();
			inputBlock.LinkToWithPropagation(increasingBlock);

			var printBlock = new ActionBlock<int>(x => Console.WriteLine($"Message {x} processed"));

			increasingBlock.LinkToWithPropagation(printBlock);

			await inputBlock.SendAsync(1);
			await inputBlock.SendAsync(2);
			await inputBlock.SendAsync(1);
			await inputBlock.SendAsync(3);
			await inputBlock.SendAsync(4);
			await inputBlock.SendAsync(2);

			inputBlock.Complete();
			await printBlock.Completion;

			Console.WriteLine("Finished!");
			Console.ReadKey();
		}

		private static IPropagatorBlock<T,T> CreateFilteringBlock<T>()
			where T : IComparable<T>, new()
		{
			T maxValue = default;
			var source = new BufferBlock<T>();
			var target = new ActionBlock<T>(async x =>
			{
				if (x.CompareTo(maxValue) > 0)
				{
					await source.SendAsync(x);
					maxValue = x;
				}
			});

			target.Completion.ContinueWith(x =>
			{
				if (x.IsFaulted)
				{
					((ITargetBlock<T>)source).Fault(x.Exception);
				}
				else
				{
					source.Complete();
				}
			});

			return DataflowBlock.Encapsulate(target, source);
		}
	}
}
