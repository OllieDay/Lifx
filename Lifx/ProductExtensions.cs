using System.Collections.Generic;
using System.Linq;

namespace Lifx
{
	internal static class ProductExtensions
	{
		private static readonly IEnumerable<Product> ColorProducts = new[]
		{
			Product.Unknown,
			Product.Original1000,
			Product.Color650,
			Product.Color1000BR30,
			Product.Color1000,
			Product.LifxPlusA19,
			Product.LifxPlusBR30,
			Product.LifxZ
		};

		public static bool SupportsColor(this Product @this)
		{
			return ColorProducts.Contains(@this);
		}
	}
}
