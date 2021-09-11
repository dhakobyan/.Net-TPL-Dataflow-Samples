using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class BatchBlockSample
	{
		public static async Task Start()
		{
			var batchBlock = new BatchBlock<int>(3);

			for (int i = 0; i < 10; i++)
			{
				batchBlock.Post(i);
			}
			batchBlock.Complete();
			batchBlock.Post(10);

			for (int i = 0; i < 5; i++)
			{
				int[] result;
				if (batchBlock.TryReceive(out result))
				{
					Console.Write($"Received batch {i}: ");

					for (int j = 0; j < result.Length; j++)
					{
						Console.Write($"{result[j]} ");
					}
					Console.WriteLine();
				}
				else
				{
					Console.WriteLine("Block is finished.");
					break;
				}
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
