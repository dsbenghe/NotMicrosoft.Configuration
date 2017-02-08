using System.Collections.Generic;
using Microsoft.Extensions.Configuration.Test.Common;
using NotMicrosoft.Configuration.Json;
using Xunit;

namespace NotMicrosoft.Configuration.Tests
{
    public class JsonTemplateConfigurationProviderTests
    {
        [Fact]
        public void Load_WithConfigValues_Applies_Values()
        {
            var json = @"{
                'setting1': '$var1$',
                'setting2': '$var2$',
                'setting3': 'value3',
            }";

            var provider = new JsonTemplateConfigurationProvider(new MockJsonTemplateConfigurationSource());
            provider.Load(TestStreamHelpers.StringToStream(json));

            Assert.Equal("var1_value", provider.Get("setting1"));
            Assert.Equal("var2_value", provider.Get("setting2"));
            Assert.Equal("value3", provider.Get("setting3"));
        }

        [Fact]
        public void Load_WithConfigValues_And_Nested_Variables_Applies_Values()
        {
            var json = @"{
                'setting1': '$var1$',
                'setting2': {
                    'subsetting1': 'subsetting1_val',
                    'subsetting2': '$var2$',
                    'subsetting3': 'f',
                }
            }";

            var provider = new JsonTemplateConfigurationProvider(new MockJsonTemplateConfigurationSource());
            provider.Load(TestStreamHelpers.StringToStream(json));

            Assert.Equal("var1_value", provider.Get("setting1"));
            Assert.Equal("var2_value", provider.Get("setting2:subsetting2"));
            Assert.Equal("subsetting1_val", provider.Get("setting2:subsetting1"));
        }

        class MockJsonTemplateConfigurationSource : JsonTemplateConfigurationSource
        {
            public override Dictionary<string, string> GetConfigValues()
            {
                return new Dictionary<string, string>()
                {
                    { "var1", "var1_value" },
                    { "var2", "var2_value" }
                };
            }
        }
    }
}
