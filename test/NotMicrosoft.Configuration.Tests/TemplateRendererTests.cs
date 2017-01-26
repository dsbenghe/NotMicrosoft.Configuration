using System.Collections.Generic;
using Xunit;

namespace NotMicrosoft.Configuration.Tests
{
    public class TemplateRendererTests
    {
        [Fact]
        public void Renders_SimpleTemplate()
        {
            var result = TemplateRenderer.RenderTemplate("blah $var$ blah2", new TemplateConfiguration(new List<string>()),
                new Dictionary<string, string> {{"var", "value1"}});

            Assert.Equal(result, "blah value1 blah2");
        }

        [Fact]
        public void Renderer_Uses_MagicCharacter()
        {
            var result = TemplateRenderer.RenderTemplate("blah #var# blah2", new TemplateConfiguration(new List<string>(), magicCharacter: '#'),
                new Dictionary<string, string> { { "var", "value1" } });

            Assert.Equal(result, "blah value1 blah2");
        }
    }
}
