using System;
using System.Text;

namespace Lifx
{
	// Represents a light label with a maximum of 32 bytes in length.
	public struct Label
	{
		public Label(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (Encoding.UTF8.GetByteCount(value) > MaxLength)
			{
				throw new ArgumentException($"Size in bytes must not exceed {MaxLength}.", nameof(value));
			}

			Value = value;
		}

		public static int MaxLength { get; } = 32;
		public static Label None { get; } = string.Empty;

		public string Value { get; }

		public static implicit operator string(Label label)
			=> label.Value;

		public static implicit operator Label(string value)
			=> new Label(value);

		public override string ToString()
			=> Value;
	}
}
