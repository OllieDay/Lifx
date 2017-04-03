using System;
using Xunit;

namespace Lifx.Tests
{
	public sealed class HueTests
	{
		[Theory]
		[InlineData(-1)]
		[InlineData(361)]
		public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenValueNotInRange(int value)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Hue(value));
		}
	}
}
