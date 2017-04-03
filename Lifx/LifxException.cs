using System;

namespace Lifx
{
	public sealed class LifxException : Exception
	{
		internal LifxException(string message) : base(message)
		{

		}

		internal LifxException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
