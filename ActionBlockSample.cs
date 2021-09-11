using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	static class ActionBlockSample
	{
		public static async Task Start()
		{
			var actionBlock = new ActionBlock<int>(x =>
			{
				Task.Delay(500).Wait();
				Console.WriteLine(x);
			});

			for (int i = 0; i < 10; i++)
			{
				actionBlock.Post(i);
				Console.WriteLine($"{actionBlock.InputCount} items in input queue.");
			}

			Console.WriteLine("Finished");
			Console.ReadKey();
		}
	}
}
