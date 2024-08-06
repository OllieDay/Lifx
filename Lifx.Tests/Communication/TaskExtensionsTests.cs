namespace Lifx.Communication.Tests;

public sealed class TaskExtensionsTests
{
	[Fact]
	public async Task WithCancellationShouldCauseTaskToThrowOperationCanceledExceptionWhenCancellationTokenIsCancelled()
	{
		using var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.Cancel();

		await Assert.ThrowsAsync<OperationCanceledException>(async () =>
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
