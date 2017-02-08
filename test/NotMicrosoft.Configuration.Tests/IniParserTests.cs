using System;
using System.Collections.Generic;
using System.IO;
using NotMicrosoft.Configuration.Parser;
using Xunit;

namespace NotMicrosoft.Configuration.Tests
{
    public class IniParserTests
    {
        [Fact]
        public void Parse_SingleFile_Works()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResurceStream("first.ini");
            var values = IniParser.Parse(firstStreamOfIniValues);
            Assert.Equal(values["var1"], "value_var1_first");
        }

        [Fact]
        public void Parse_TwoFiles_Works()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResurceStream("first.ini");
            var secondStreamOfIniValues = ResourceHelper.GetResurceStream("second.ini");
            var values = IniParser.Parse(new List<Stream> { firstStreamOfIniValues, secondStreamOfIniValues});
            Assert.Equal(values["var2"], "value_var2_first");
            Assert.Equal(values["var3"], "value_var3_second");
        }

        [Fact]
        public void Parse_TwoFiles_SecondOverridesFirst()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResurceStream("first.ini");
            var secondStreamOfIniValues = ResourceHelper.GetResurceStream("second.ini");
            var values = IniParser.Parse(new List<Stream> { firstStreamOfIniValues, secondStreamOfIniValues });
            Assert.Equal(values["var1"], "value_var1_second");
            Assert.Equal(values["var2"], "value_var2_first");
            Assert.Equal(values["var3"], "value_var3_second");
        }

        [Fact]
        public void Parse_InvalidFile_Throws()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResurceStream("invalid.ini");
            Assert.Throws<ArgumentException>(() => IniParser.Parse(firstStreamOfIniValues));
        }
    }
}
