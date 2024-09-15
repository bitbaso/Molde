using Microsoft.Extensions.Logging;
using Molde.Entities;
using Molde.Features.Actions.Interfaces;
using Molde.Features.PromptManager.Interfaces;

namespace Molde.Features.PromptManager;

public class PromptManager(ILogger<PromptManager> logger, IActionsManager actionsManager) : IPromptManager
{
    #region Public functions
    public async Task<bool> RunPromptAsync(List<ConfigEntity> configurations)
    {
        try
        {
            var outcome = default(bool);

            if (configurations != null && configurations.Count > 0)
            {
                if (configurations.Count > 1)
                {
                    Console.WriteLine("Choose:");
                    foreach (var configuration in configurations)
                    {
                        if (configuration != null)
                        {
                            Console.WriteLine($"{configuration.Id} - {configuration.Name}");
                        }
                    }
                    Console.WriteLine("Option:");
                    var selectedOption = Console.ReadLine();
                    if (!string.IsNullOrEmpty(selectedOption))
                    {
                        var parseOK = int.TryParse(selectedOption, out var option);
                        if (parseOK)
                        {
                            var configurationToExecute = configurations.FirstOrDefault(configuration => configuration.Id == option);
                            if (configurationToExecute != null)
                            {
                                outcome = await ExecutePromptForAConfiguration(configurationToExecute);
                            }
                        }
                    }
                }
                else
                {
                    outcome = await ExecutePromptForAConfiguration(configurations[0]);
                }

            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR RunPromptAsync");
            return default;
        }
    }

    #endregion Public functions

    #region Private functions
    private async Task<bool> ExecutePromptForAConfiguration(ConfigEntity configuration)
    {
        try
        {
            var outcome = default(bool);

            if (configuration != null)
            {
                Console.WriteLine($"Executing {configuration.Id} - {configuration.Name}");
                var variables = new Dictionary<string, string>();

                if (configuration.Prompts != null && configuration.Prompts.Count > 0)
                {
                    foreach (var prompt in configuration.Prompts)
                    {
                        Console.WriteLine(prompt.Message);
                        string response = Console.ReadLine();
                        variables[prompt.Name] = response;
                    }
                }

                var actionsExecuted = await actionsManager.ExecuteActions(configuration.Actions, variables);

                if (actionsExecuted)
                {
                    Console.WriteLine("Actions completed successfully.");
                    outcome = true;
                }
                else
                {
                    Console.WriteLine("Actions NOT completed.");
                    outcome = false;
                }

            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  ExecutePromptForAConfiguration");
            return default;
        }
    }

    #endregion Private functions
}
