using Xunit;

namespace Loft.Specs
{
    public class ReflectionExtensionsSpecs
    {
        [Fact]
        public void Can_get_data_from_private_field()
        {
            TestDocument document = new TestDocument{Id = "abc"};
            string value = document.GetValue("Id");
            Assert.Equal("abc", value);
        }

        [Fact]
        public void Can_set_value()
        {
            TestDocument document = new TestDocument();
            document.SetValue("Id", "abc");
            Assert.Equal("abc", document.Id);
        }
    }
}