using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class TransformBlockParallelSample
	{
		public static async Task Start()
		{
			var transformBlock = new TransformBlock<int, string>(x =>
			{
				Task.Delay(500).Wait();
				return x.ToString();
			},
			new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2 });

			for (int i = 0; i < 10; i++)
			{
				transformBlock.Post(i);
				Console.WriteLine($"{transformBlock.InputCount} items in input queue.");
			}

			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine($"{transformBlock.OutputCount} items in output queue, {transformBlock.InputCount} in input queue.");
				var result = transformBlock.Receive();
				Console.WriteLine($"Received: {result}");
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
