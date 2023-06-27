using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Common;

public class TestBase : IClassFixture<TestFactory<Program>>
{
    public readonly TestFactory<Program> Factory;

    public TestBase(TestFactory<Program> factory)
    {
        Factory = factory;
    }
}