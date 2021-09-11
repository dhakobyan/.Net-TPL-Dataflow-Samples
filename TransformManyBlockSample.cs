using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class TransformManyBlockSample
	{
		public static async Task Start()
		{
			var transformManyBlock = new TransformManyBlock<int, string>(x => FindEvenNumbers(x),
				new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2 });
			var printBlock = new ActionBlock<string>(x => Console.WriteLine($"Received: {x}"));

			//link source to consumer
			transformManyBlock.LinkTo(printBlock);

			for (int i = 0; i < 10; i++)
			{
				transformManyBlock.Post(i);
				Console.WriteLine($"{transformManyBlock.InputCount} items in input queue.");
			}
			
			Console.WriteLine("Finished");
			Console.ReadKey();
		}

		private static IEnumerable<string> FindEvenNumbers(int number)
		{
			for (int i = 0; i < number; i++)
			{
				if (i % 2 == 0)
				{
					yield return $"{number}:{i}";
				}
			}
			
		}
	}
}
