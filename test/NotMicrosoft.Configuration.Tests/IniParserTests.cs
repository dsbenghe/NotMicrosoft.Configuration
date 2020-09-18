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
        public void Parse_when_single_file_return_name_value()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResourceStream("first.ini");
            var values = IniParser.Parse(firstStreamOfIniValues);
            Assert.Equal("value_var1_first", values["var1"]);
        }

        [Fact]
        public void Parse_when_two_files_return_name_value()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResourceStream("first.ini");
            var secondStreamOfIniValues = ResourceHelper.GetResourceStream("second.ini");
            var values = IniParser.Parse(new List<Stream> {firstStreamOfIniValues, secondStreamOfIniValues});
            Assert.Equal("value_var2_first", values["var2"]);
            Assert.Equal("value_var3_second", values["var3"]);
        }

        [Fact]
        public void Parse_when_two_files_values_in_second_overrides_first()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResourceStream("first.ini");
            var secondStreamOfIniValues = ResourceHelper.GetResourceStream("second.ini");
            var values = IniParser.Parse(new List<Stream> {firstStreamOfIniValues, secondStreamOfIniValues});
            Assert.Equal("value_var1_second", values["var1"]);
            Assert.Equal("value_var2_first", values["var2"]);
            Assert.Equal("value_var3_second", values["var3"]);
        }

        [Fact]
        public void Parse_when_invalid_file_throws()
        {
            var firstStreamOfIniValues = ResourceHelper.GetResourceStream("invalid.ini");
            Assert.Throws<ArgumentException>(() => IniParser.Parse(firstStreamOfIniValues));
        }
    }
}