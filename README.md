# NotMicrosoft.Configuration

[![Build status](https://ci.appveyor.com/api/projects/status/3j4o0h0o47ayrjtk/branch/master?svg=true)](https://ci.appveyor.com/project/dsbenghe/notmicrosoft-configuration/branch/master) - Windows Build

[![Build Status](https://travis-ci.org/dsbenghe/NotMicrosoft.Configuration.svg?branch=master)](https://travis-ci.org/dsbenghe/NotMicrosoft.Configuration) - Linux

[![NuGet](https://img.shields.io/nuget/v/NotMicrosoft.Configuration.svg)](https://www.nuget.org/packages/NotMicrosoft.Configuration/)

Extending the configuration posibilities of .NET core apps via json by using configurable "variables" (e.g. $connection_string$) whose values are stored in ini/properties files (e.g. making it a better solution for deploying production apps :)). Fully compatible with the built-in json configuration provider - designed to work as a drop-in replacement for it.

## Background
Microsoft introduced with .NET Core the Configuration API as a way of configuring an application. An application configuration must have an easy way of changing it depending on the environment (qa, staging, production, whatever) where the application is running.

This new way of configuring an application is way better and more flexible than the older ways (for example web.config and the horrible transformation file - horrible even if you love xml & its children :) - but this probably deserves its own rant), but it still has gaps and is unnecessary messy to achieve some simple changes - which is surprising considering that this is easy on other platforms (e.g. for example in java spring & properties files)

## The problem
Considering the example of configuring Serilog in appsettings.json file and the fact that we want to overwrite this configuration when running in production - we want to overwrite the MinimumLevel and the path where the log files will be stored (this is quite expected and in fact exactly what is asked on stack overflow in one of the questions - http://stackoverflow.com/questions/37657320/how-to-override-an-asp-net-core-configuration-array-setting-using-environment-va)

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Literate" ],
    "MinimumLevel": "Debug",
    "WriteTo": [
      { "Name": "LiterateConsole" },
      { "Name": "Trace" },
      {
        "Name": "RollingFile",
        "Args": {
          "pathFormat": "./Log/Log-{Date}.txt",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {MachineName} {RequestId} ({ThreadId}) [{Level}] - {Message}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  }
}
```

Using Microsoft configuration api, there are couple of options for this (as far as I know):
* Create appsettings.envname.json and repeat the WHOLE!!! Serilog json block again with the new values for MinimumLevel and pathFormat - this is probably the worst thing you can do
* Overwrite via ini file or environment variable by using the configuration hierarchy (this is what is suggested in the answer to the stack overflow question) e.g.
  * Serilog:MinimumLevel=Information
  * Serilog:WriteTo:0:Args:pathFormat={some other path}


Now, this is better than the json overwrite but still too messy:
* this json block is very often represented in code by some type and the json is mapped automatically to the type. Any change in that type structure needs to be reflected in changes in appsettings.json (and this is probably expected by the dev team), but in our case will trigger changes in environment specific files/config (ini file and/or environment variables). These files are possible owned by other teams (production configuration values). So a change in the configuration type can trigger a chain reaction. Now, you can "template" the ini files and create some other configuration names which are the environment specific ones (anything can be solved with another level of indirection :), isn't it) - but why complicate yourself.
* could look just like random crap e.g. Serilog:WriteTo:0:Args:pathFormat - how this makes sense? :)
* if you just want to just partially configure a value - this is not possible. For example, if we have a connection string and we just want to modify the password for the database user (as all the other values are the same between environments). This is not possible using the configuration api and the whole connection string needs to be set together with the new password.

## The solution
New json configuration provider which makes appsettings.json to work as a template whose "variables" are replaced via name-values pairs from a ini file. For example, the Serilog configuration will look like this

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Literate" ],
    "MinimumLevel": "$log_level$",
    "WriteTo": [
      { "Name": "LiterateConsole" },
      { "Name": "Trace" },
      {
        "Name": "RollingFile",
        "Args": {
          "pathFormat": "$log_path$",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {MachineName} {RequestId} ({ThreadId}) [{Level}] - {Message}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  }
}
```

Underneath the provider is using StringTemplate to do the replacements. I would have prefereed to use the spring syntax ${var}, but this is not supported by StringTemplate.

See sample for basic usage.
