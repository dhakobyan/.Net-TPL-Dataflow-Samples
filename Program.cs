using System;
using System.Threading.Tasks;

namespace TPL.Dataflow.Samples
{
	class Program
	{
		static async Task Main(string[] args)
		{
			//await ActionBlockSample.Start();
			//await TransformBlockSample.Start();
			//await TransformBlockParallelSample.Start();
			//await BatchBlockSample.Start();
			//await TransformManyBlockSample.Start();
			//await BufferBlockSample.Start();
			//await BufferBlockBounedSample.Start();
			//await BroadcastBlockSample.Start();
			//await JoinBLockSample.Start();
			//await JoinBlockParallelSample.Start();
			//await BatchedJoinBlockSample.Start();
			//await WriteOnceBlockSample.Start();
			//await WriteOnceBlockExplanationSample.Start();
			//await CompletionSample.Start();
			//await CompletionPropagationSample.Start();
			//await LinkToOptionsSample.Start();
			//await MultipleProducersSample.Start();
			//await MultipleProducersCompletionSample.Start();
			//await ErrorHandlingSample.Start();
			//await EncapsulatedCustomBlockSample.Start();
			await GuaranteedDeliveryBroadcastCustomBlockSample.Start();
		}
	}
}
