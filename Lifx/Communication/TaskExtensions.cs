using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lifx.Communication;

internal static class TaskExtensions
{
	// Allows a non-cancellable Task<T> to be cancelled.
	public static async Task<T> WithCancellation<T>(this Task<T> @this, CancellationToken token)
	{
		var source = new TaskCompletionSource<object>();

		using (token.Register(() => source.TrySetResult(null), useSynchronizationContext: false))
		{
			if (@this != await Task.WhenAny(@this, source.Task).ConfigureAwait(false))
			{
				throw new OperationCanceledException(token);
			}
		}

		return await @this.ConfigureAwait(false);
	}
}
