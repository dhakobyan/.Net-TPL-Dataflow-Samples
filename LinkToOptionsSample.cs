using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class LinkToOptionsSample
	{
		public static async Task Start()
		{
			var bufferBlock = new BufferBlock<int>();

			var consumer1 = new ActionBlock<int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 1");
				Task.Delay(300).Wait();
			});
			var consumer2 = new ActionBlock<int>(x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 2");
				Task.Delay(300).Wait();
			});

			//link producer with consumers
			bufferBlock.LinkTo(consumer1,
				x => x % 2 == 0); //filtering
			bufferBlock.LinkTo(consumer2,
				new DataflowLinkOptions
				{
					Append = false, //prepends the consumer
					MaxMessages = 5
				});

			//to accept and discard all messages that are not accepted by previous targets
			
			//use null target
			//bufferBlock.LinkTo(DataflowBlock.NullTarget<int>());

			//OR
			//use a logger block for further troubleshooting
			bufferBlock.LinkTo(new ActionBlock<int>(x => Console.WriteLine($"Message {x} discarded")));

			for (int i = 0; i < 10; i++)
			{
				bufferBlock.Post(i);
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
