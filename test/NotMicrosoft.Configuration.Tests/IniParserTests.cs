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
            Assert.Equal("value_var1_first", values["var1"]);
        }

        [Fact]
        public void Parse_TwoFiles_Works()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResurceStream("first.ini");
            var secondStreamOfIniValues = ResourceHelper.GetResurceStream("second.ini");
            var values = IniParser.Parse(new List<Stream> { firstStreamOfIniValues, secondStreamOfIniValues});
            Assert.Equal("value_var2_first", values["var2"]);
            Assert.Equal("value_var3_second", values["var3"]);
        }

        [Fact]
        public void Parse_TwoFiles_SecondOverridesFirst()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResurceStream("first.ini");
            var secondStreamOfIniValues = ResourceHelper.GetResurceStream("second.ini");
            var values = IniParser.Parse(new List<Stream> { firstStreamOfIniValues, secondStreamOfIniValues });
            Assert.Equal("value_var1_second", values["var1"]);
            Assert.Equal("value_var2_first", values["var2"]);
            Assert.Equal("value_var3_second", values["var3"]);
        }

        [Fact]
        public void Parse_InvalidFile_Throws()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResurceStream("invalid.ini");
            Assert.Throws<ArgumentException>(() => IniParser.Parse(firstStreamOfIniValues));
        }
    }
}
