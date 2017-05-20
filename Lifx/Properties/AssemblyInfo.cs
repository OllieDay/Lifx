using System.Runtime.CompilerServices;

// Allow Lifx.Tests access to internal types
[assembly: InternalsVisibleTo("Lifx.Tests")]

// Allow CastleProxy (used by Moq) access to internal types
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
