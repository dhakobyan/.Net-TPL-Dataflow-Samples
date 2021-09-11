using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class WriteOnceBlockSample
	{
		public static async Task Start()
		{
			var writeOnceBlock = new WriteOnceBlock<int>(x => x);

			for (int i = 0; i < 10; i++)
			{
				if (writeOnceBlock.Post(i))
				{
					Console.WriteLine($"Message {i} was accepted");
				}
				else
				{
					Console.WriteLine($"Message {i} was rejected");
				}
			}

			for (int i = 0; i < 15; i++)
			{
				if (writeOnceBlock.TryReceive(out var res))
				{
					Console.WriteLine($"Message {res} received. Iteration: {i}");
				}
				else
				{
					Console.WriteLine("No more messages.");
				}
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
