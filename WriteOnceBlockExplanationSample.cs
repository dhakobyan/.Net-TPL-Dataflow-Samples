using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class WriteOnceBlockExplanationSample
	{
		public static async Task Start()
		{
			var writeOnceBlock = new WriteOnceBlock<int>(x => x);

			// push vs pull
			//consumer is being offered a message
			var print = new ActionBlock<int>(x => Console.WriteLine($"Message {x} received."));

			writeOnceBlock.LinkTo(print);

			for (int i = 0; i < 10; i++)
			{
				if (writeOnceBlock.Post(i))
					Console.WriteLine($"Message {i} was accepted");
				else
					Console.WriteLine($"Message {i} was rejected");
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
