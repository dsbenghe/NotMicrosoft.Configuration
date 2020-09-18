using System.Collections.Generic;
using Xunit;

namespace NotMicrosoft.Configuration.Tests
{
    public class TemplateRendererTests
    {
        [Fact]
        public void Render_renders_simple_template()
        {
            var result = TemplateRenderer.RenderTemplate("blah $var$ blah2",
                new TemplateConfiguration(new List<string>()),
                new Dictionary<string, string> {{"var", "value1"}});

            Assert.Equal("blah value1 blah2", result);
        }

        [Fact]
        public void Render_when_non_default_magic_char_replace_value()
        {
            var result = TemplateRenderer.RenderTemplate("blah #var# blah2",
                new TemplateConfiguration(new List<string>(), magicCharacter: '#'),
                new Dictionary<string, string> {{"var", "value1"}});

            Assert.Equal("blah value1 blah2", result);
        }
    }
}