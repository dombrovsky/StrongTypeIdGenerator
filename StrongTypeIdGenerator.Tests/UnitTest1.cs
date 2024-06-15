namespace StrongTypeIdGenerator.Tests
{
    public class StrongTypeIdTests
    {
        [SetUp]
        public void Setup()
        {
            var a = MyIdClass.Unspecified;
            var b = new MyIdClass("1");
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }

    [StringId(GenerateConstructorPrivate = false)]
    public sealed partial class MyIdClass
    {
    }

    [GuidId]
    public sealed partial class MyGuidIdClass
    {
    }
}