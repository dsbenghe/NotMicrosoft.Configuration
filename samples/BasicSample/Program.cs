using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NotMicrosoft.Configuration;

namespace BasicSample
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonTemplateFile("appsettings.json", (options) =>
                {
                    options.IniFilePath = "settings.ini";
                });

            Configuration = builder.Build();

            Console.WriteLine($"option1 = {Configuration["option1"]}");
            Console.WriteLine($"option2 = {Configuration["option2"]}");
            Console.WriteLine($"suboption1 = {Configuration["subsection:suboption1"]}");
            Console.WriteLine($"suboption2 = {Configuration["subsection:suboption2"]}");
        }
    }
}