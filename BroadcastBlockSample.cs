using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class BroadcastBlockSample
	{
		public static async Task Start()
		{
			var broadcastBlock = new BroadcastBlock<int>(x => x,
				new DataflowBlockOptions { BoundedCapacity = 1 });

			var consumer1 = new ActionBlock<int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 1");
				Task.Delay(300).Wait();
			}, new ExecutionDataflowBlockOptions{ BoundedCapacity = 1 }
			);
			var consumer2 = new ActionBlock<int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 2");
				Task.Delay(300).Wait();
			}, new ExecutionDataflowBlockOptions { BoundedCapacity = 1 }
			);

			//link producer with consumers
			broadcastBlock.LinkTo(consumer1);
			broadcastBlock.LinkTo(consumer2);

			for (int i = 0; i < 10; i++)
			{
				//bufferBlock.Post(i);

				//if (!bufferBlock.Post(i))
				//{
				//	Console.WriteLine($"Message {i} was rejected.");
				//}

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

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
