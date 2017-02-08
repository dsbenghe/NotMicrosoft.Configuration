using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using NotMicrosoft.Configuration;
using NotMicrosoft.Configuration.Json;

namespace BasicSample
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonTemplateFile("appsettings.json", 
                    new TemplateConfiguration(new List<string> {"settings.ini", "settings.prod.ini"},
                        "WHATEVER_CONFIG", // default NOTMICROSOFT_CONFIG
                        '$' // default is '$'
                        )
                );

            Configuration = builder.Build();

            Console.WriteLine($"option1 = {Configuration["option1"]}");
            Console.WriteLine($"option2 = {Configuration["option2"]}");
            Console.WriteLine($"option3 = {Configuration["option3"]}");
            Console.WriteLine($"suboption1 = {Configuration["subsection:suboption1"]}");
            Console.WriteLine($"suboption2 = {Configuration["subsection:suboption2"]}");
        }
    }
}