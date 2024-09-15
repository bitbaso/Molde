using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Molde.Features.ConfigFiles.Interfaces;
using Molde.Features.ConfigFiles;
using Molde.Features.Actions.Interfaces;
using Molde.Features.Actions;
using Molde.Features.FormatCases.Interfaces;
using Molde.Features.FormatCases;
using Molde.Features.PromptManager;
using Molde.Features.PromptManager.Interfaces;

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IConfigFilesManager, ConfigFilesManager>()
            .AddSingleton<IActionsManager, ActionsManager>()
            .AddSingleton<IFormatCasesManager, FormatCasesManager>()
            .AddSingleton<IPromptManager, PromptManager>()
            .BuildServiceProvider();

        // Resolver el servicio
        var formatCasesManager = serviceProvider.GetService<IFormatCasesManager>();
        var configFilesManager = serviceProvider.GetService<IConfigFilesManager>();
        var promptManager = serviceProvider.GetService<IPromptManager>();

        var registeredCases = await formatCasesManager.RegisterHelpers();

        if (registeredCases)
        {
            var configPath = "molde.json";

            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--config" && i + 1 < args.Length)
                    {
                        configPath = args[i + 1];
                        i++;
                    }
                }
            }

            Console.WriteLine($"Config json path: {configPath}");

#if DEBUG
            configPath = "TestData/molde.json";
#endif

            var configurations = await configFilesManager?.LoadConfigFilesAsync(configPath);

            if (configurations != null)
            {
                await promptManager.RunPromptAsync(configurations);
            }
        }
    }
}

