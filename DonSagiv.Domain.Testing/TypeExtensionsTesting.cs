namespace DonSagiv.Domain.Testing;

public class TypeExtensionsTesting
{
    [Theory]
    [InlineData(typeof(TestImplementation), typeof(ITestInterface)))]
    public void Implements_Should_ReturnTrueIfCompatible(Type derivedType, Type baseType)
    {
        // Act
        var isImplementation = derivedType.IsAssignableFrom(baseType);


    }
}

public interface ITestInterface
{
    public string foo { get; set; }
}

public abstract class TestBase
{
    public int bar { get; set; }
}

public class TestImplementation : TestBase, ITestInterface
{
    public string foo { get; set; }
    public int bar { get; set; }
}
