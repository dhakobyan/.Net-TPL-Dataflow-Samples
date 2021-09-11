using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class MultipleProducersCompletionSample
	{
		public static async Task Start()
		{
			//two producers
			var producer1 = new TransformBlock<string, string>(async x =>
			{
				await Task.Delay(300);
				return x;
			});
			var producer2 = new TransformBlock<string, string>(async x =>
			{
				await Task.Delay(500); //produces messages slower than the other one
				return x;
			});

			//one consumer
			var printBlock = new ActionBlock<string>(x => Console.WriteLine(x));

			producer1.LinkTo(printBlock);
			producer2.LinkTo(printBlock);

			for (int i = 0; i < 10; i++)
			{
				await producer1.SendAsync($"Producer 1 Message {i}");
				await producer2.SendAsync($"Producer 2 Message {i}");
			}

			producer1.Complete();
			producer2.Complete();

			Task.WhenAll(producer1.Completion, producer2.Completion)
				.ContinueWith(x => printBlock.Complete());

			await printBlock.Completion;

			Console.WriteLine("Finished!");
			Console.ReadKey();
		}
	}
}
