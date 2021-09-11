using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class CompletionPropagationSample
	{
		public static async Task Start()
		{
			var broadcastBlock = new BroadcastBlock<int>(x => x,
				new DataflowBlockOptions { BoundedCapacity = 1 });

			var consumer1 = new TransformBlock<int, int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 1");
				if (x % 2 == 0)
				{
					Task.Delay(300).Wait();
				}
				else
				{
					Task.Delay(50).Wait();
				}
				return -1 * x;
			}
			, new ExecutionDataflowBlockOptions{ MaxDegreeOfParallelism = 2 });

			var consumer2 = new TransformBlock<int, int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 2");
				if (x % 2 != 0)
				{
					Task.Delay(300).Wait();
				}
				else
				{
					Task.Delay(50).Wait();
				}
				return x;
			}
			, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2 });

			//link producer with consumers
			broadcastBlock.LinkToWithPropagation(consumer1);
			broadcastBlock.LinkToWithPropagation(consumer2);

			var joinBlock = new JoinBlock<int, int>();
			consumer1.LinkToWithPropagation(joinBlock.Target1);
			consumer2.LinkToWithPropagation(joinBlock.Target2);

			var finalBlock = new ActionBlock<Tuple<int, int>>(
				x => Console.WriteLine($"Message {x} was processed by ALL consumers. Sum was {x.Item1 + x.Item2}"));
			joinBlock.LinkToWithPropagation(finalBlock);

			for (int i = 0; i < 10; i++)
			{
				await broadcastBlock.SendAsync(i)
					.ContinueWith(x =>
					{
						if (x.Result)
						{
							Console.WriteLine($"Message {i} was accepted");
						}
						else
						{
							Console.WriteLine($"Message {i} was rejected");
						}
					});
			}

			broadcastBlock.Completion.ContinueWith(x => Console.WriteLine("Producing finished."));

			//we will not produce messages any more
			broadcastBlock.Complete();

			//do not end the program until the last block is completed
			await finalBlock.Completion;

			Console.WriteLine("Finished");
		}
	}
}
