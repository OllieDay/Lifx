using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Lifx.Communication.Tests
{
	public sealed class TaskExtensionsTests
	{
		[Fact]
		public void WithCancellationShouldCauseTaskToThrowOperationCanceledExceptionWhenCancellationTokenIsCancelled()
		{
			using (var cancellationTokenSource = new CancellationTokenSource())
			{
				cancellationTokenSource.Cancel();

				Assert.ThrowsAsync<OperationCanceledException>(async () =>
				{
					// Will never complete
					Func<object> function = () =>
					{
						Task.Delay(Timeout.InfiniteTimeSpan).GetAwaiter().GetResult();

						return null;
					};

					await Task.Run(function).WithCancellation(cancellationTokenSource.Token);
				});
			}
		}
	}
}
