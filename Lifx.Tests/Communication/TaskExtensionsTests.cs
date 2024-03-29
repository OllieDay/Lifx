namespace Lifx.Communication.Tests;

public sealed class TaskExtensionsTests
{
	[Fact]
	public void WithCancellationShouldCauseTaskToThrowOperationCanceledExceptionWhenCancellationTokenIsCancelled()
	{
		using var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.Cancel();

		Assert.ThrowsAsync<OperationCanceledException>(async () =>
		{
			// Will never complete
			static object? Function()
			{
				Task.Delay(Timeout.InfiniteTimeSpan).GetAwaiter().GetResult();

				return null;
			}

			await Task.Run(Function).WithCancellation(cancellationTokenSource.Token);
		});
	}
}
