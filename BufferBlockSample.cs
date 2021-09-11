using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class BufferBlockSample
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
			bufferBlock.LinkTo(consumer1);
			bufferBlock.LinkTo(consumer2);

			for (int i = 0; i < 10; i++)
			{
				bufferBlock.Post(i);
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
