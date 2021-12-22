using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class BufferBlockBounedSample
	{
		public static async Task Start()
		{
			var bufferBlock = new BufferBlock<int>(
				new DataflowBlockOptions { BoundedCapacity = 3 });

			Stopwatch sw = Stopwatch.StartNew();

			var consumer1 = new ActionBlock<int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 1 ({sw.ElapsedMilliseconds})");
				Task.Delay(3500).Wait();
			}, new ExecutionDataflowBlockOptions{ BoundedCapacity = 1 });
			var consumer2 = new ActionBlock<int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 2 ({sw.ElapsedMilliseconds})");
				Task.Delay(2500).Wait();
			}, new ExecutionDataflowBlockOptions { BoundedCapacity = 1 });

			//link producer with consumers
			bufferBlock.LinkTo(consumer1);
			bufferBlock.LinkTo(consumer2);

			for (int i = 0; i < 10; i++)
			{
				var value = i;
				//bufferBlock.Post(value);

				//if (!bufferBlock.Post(value))
				//{
				//	Console.WriteLine($"Message {value} was rejected.");
				//}

				await bufferBlock.SendAsync(value)
					.ContinueWith(x =>
					{
						if (x.Result)
						{
							Console.WriteLine($"({sw.ElapsedMilliseconds}) Message {value} was accepted");
						}
						else
						{
							Console.WriteLine($"({sw.ElapsedMilliseconds}) Message {value} was rejected");
						}
					});
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
