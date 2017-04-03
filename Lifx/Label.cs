using System;
using System.Text;

namespace Lifx
{
	// Represents a light label with a maximum of 32 bytes in length.
	public struct Label
	{
		public Label(string value)
		{
			if (Encoding.UTF8.GetByteCount(value) > MaxLength)
			{
				throw new ArgumentException($"Size in bytes must not exceed {MaxLength}.", nameof(value));
			}

			Value = value;
		}

		public static int MaxLength => 32;

		public string Value { get; }

		internal static Label None => string.Empty;

		public static implicit operator string(Label label)
		{
			return label.Value;
		}

		public static implicit operator Label(string value)
		{
			return new Label(value);
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
