using Xunit;

namespace Common;

public abstract class TestBase<TFactory> : IClassFixture<TFactory> where TFactory : class, new() { }