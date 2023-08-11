namespace Lifx.Tests
{
	public sealed class PercentageTests
	{

		[Theory]
		[InlineData(-0.1)]
		[InlineData(1.1)]
		public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenValueNotInRange(double value)
			=> Assert.Throws<ArgumentOutOfRangeException>(() => new Percentage(value));
	}
}
