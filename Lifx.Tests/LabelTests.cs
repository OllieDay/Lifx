using System;
using Xunit;

namespace Lifx.Tests
{
	public sealed class LabelTests
	{
		[Theory]
		[InlineData("000000000000000000000000000000000")]
		[InlineData("零零零零零零零零零零零")]
		public void ConstructorShouldThrowArgumentExceptionWhenNumberOfBytesExceedsMaxLength(string value)
			=> Assert.Throws<ArgumentException>(() => new Label(value));
	}
}
