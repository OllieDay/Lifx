using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lifx.Communication.Requests;
using Xunit;

namespace Lifx.Communication.Responses.Payloads.Tests
{
	public sealed class StateVersionResponsePayloadParserTests
	{
		private static readonly StateVersionResponsePayloadParser Parser = new StateVersionResponsePayloadParser();

		[Fact]
		public void ParseShouldReturnPayloadWithValidVendor()
		{
			const uint vendor = ushort.MaxValue;

			var data = CreatePayloadData(vendor, Product.Unknown, 0);

			Parser.Parse(data).Vendor.Should().Be(vendor);
		}

		[Fact]
		public void ParseShouldReturnPayloadWithUnknownVendorWhenProductDataIsInvalid()
		{
			var product = (Product)ushort.MaxValue;

			var data = CreatePayloadData(0, product, 0);

			Parser.Parse(data).Product.Should().Be(Product.Unknown);
		}

		[Fact]
		public void ParseShouldReturnPayloadWithValidProduct()
		{
			var product = Product.Color1000;

			var data = CreatePayloadData(0, product, 0);

			Parser.Parse(data).Product.Should().Be(product);
		}

		[Fact]
		public void ParseShouldReturnPayloadWithValidVersion()
		{
			const uint version = ushort.MaxValue;

			var data = CreatePayloadData(0, Product.Unknown, version);

			Parser.Parse(data).Version.Should().Be(version);
		}

		private static byte[] CreatePayloadData(uint vendor, Product product, uint version)
		{
			var vendorData = vendor.GetBytes();
			var productData = ((uint)product).GetBytes();
			var versionData = version.GetBytes();

			return CombineArrays(vendorData, productData, versionData);
		}

		private static byte[] CombineArrays(params byte[][] arrays)
			=> arrays.SelectMany(array => array).ToArray();
	}
}
