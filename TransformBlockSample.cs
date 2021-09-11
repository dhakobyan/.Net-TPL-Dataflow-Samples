using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class TransformBlockSample
	{
		public static async Task Start()
		{
			var transformBlock = new TransformBlock<int, string>(x =>
			{
				Task.Delay(500).Wait();
				return x.ToString();
			});

			for (int i = 0; i < 10; i++)
			{
				transformBlock.Post(i);
				Console.WriteLine($"{transformBlock.InputCount} items in input queue.");
			}

			for (int i = 0; i < 10; i++)
			{
				var result = transformBlock.Receive();
				Console.WriteLine($"Received: {result}");
				Console.WriteLine($"{transformBlock.OutputCount} items in output queue.");
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
