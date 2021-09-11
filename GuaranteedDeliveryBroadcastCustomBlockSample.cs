using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.Dataflow.Samples
{
	public static class GuaranteedDeliveryBroadcastCustomBlockSample
	{
		public static async Task Start()
		{
			var broadcastBlock = new GuaranteedDeliveryBroadcastBlock<int>(x => x);

			var consumer1 = new ActionBlock<int>(async x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 1");
				await Task.Delay(500);
			}, new ExecutionDataflowBlockOptions { BoundedCapacity = 1 }
			);
			var consumer2 = new ActionBlock<int>(async x =>
			{
				Console.WriteLine($"Message {x} consumed by Consumer 2");
				await Task.Delay(150);
			}, new ExecutionDataflowBlockOptions { BoundedCapacity = 1 }
			);

			broadcastBlock.LinkToWithPropagation(consumer1);
			broadcastBlock.LinkToWithPropagation(consumer2);

			for (int i = 0; i < 10; i++)
			{
				await broadcastBlock.SendAsync(i);
			}

			broadcastBlock.Complete();
			await consumer1.Completion;
			await consumer2.Completion;

			Console.WriteLine("Finished!");
			Console.ReadKey();
		}
	}
#nullable enable
	class GuaranteedDeliveryBroadcastBlock<T> : IPropagatorBlock<T, T>
	{
		private readonly BroadcastBlock<T> _broadcastBlock;
		private Task _completion;

		public GuaranteedDeliveryBroadcastBlock(Func<T, T>? cloningFunction)
		{
			_broadcastBlock = new BroadcastBlock<T>(cloningFunction);
			_completion = _broadcastBlock.Completion;
		}
		public Task Completion => _completion;

		public void Complete()
		{
			_broadcastBlock.Complete();
		}

		public void Fault(Exception exception)
		{
			((ITargetBlock<T>)_broadcastBlock).Fault(exception);
		}

		public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
		{
			var bufferBLock = new BufferBlock<T>();
			var d1 = _broadcastBlock.LinkTo(bufferBLock, linkOptions);
			var d2 = bufferBLock.LinkTo(target, linkOptions);

			_completion.ContinueWith(x => bufferBLock.Completion);

			return new DisposableDisposer(d1, d2);
		}

		public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T>? source, bool consumeToAccept)
		{
			return ((ITargetBlock<T>)_broadcastBlock).OfferMessage(messageHeader, messageValue, source, consumeToAccept);
		}

		public T? ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
		{
			throw new NotSupportedException("This method should not be called. The producer is the BufferBlock!");
		}

		public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			throw new NotSupportedException("This method should not be called. The producer is the BufferBlock!");
		}

		public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			throw new NotSupportedException("This method should not be called. The producer is the BufferBlock!");
		}
	}

	class DisposableDisposer : IDisposable
	{
		private readonly IDisposable[] _disposables;

		public DisposableDisposer(params IDisposable[] disposables)
		{
			_disposables = disposables;
		}
		public void Dispose()
		{
			foreach (var disposable in _disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
