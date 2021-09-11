using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class ErrorHandlingSample
	{
		public static async Task Start()
		{
			var block = new TransformBlock<int, string>(x =>
			{
				if (x == 5)
				{
					throw new Exception("Something went wrong");
				}
				Console.WriteLine($"Message {x} processed");
				return x.ToString();
			});

			var printBlock = new ActionBlock<string>(x => Console.WriteLine($"Message {x} printed"));
			block.LinkToWithPropagation(printBlock);

			for (int i = 0; i < 10; i++)
			{
				if (await block.SendAsync(i))
				{
					Console.WriteLine($"Message {i} was accepted");
				}
				else
				{
					Console.WriteLine($"Message {i} was rejected");
				}
			}

			block.Complete();
			try
			{
				await printBlock.Completion;
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten().InnerException;
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
