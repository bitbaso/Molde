using System.Text.Json;
using Molde.Entities;
using Molde.Features.ConfigFiles.Interfaces;
using Microsoft.Extensions.Logging;

namespace Molde.Features.ConfigFiles;

public class ConfigFilesManager(ILogger<ConfigFilesManager> logger) : IConfigFilesManager
{
    #region Public functions
    /// <summary>
    /// Loads the configuration from file path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<List<ConfigEntity>> LoadConfigFilesAsync(string path)
    {
        try
        {
            var outcome = default(List<ConfigEntity>);

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                outcome = new List<ConfigEntity>();
                var jsonConfig = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(jsonConfig))
                {
                    var firstModelConfiguration = JsonSerializer.Deserialize<ConfigFileEntity>(jsonConfig);
                    if (firstModelConfiguration != null)
                    {
                        if (firstModelConfiguration.MoldeFiles != null && firstModelConfiguration.MoldeFiles.Any())
                        {
                            foreach (var moldeFile in firstModelConfiguration.MoldeFiles)
                            {
                                var moldeConfigEntities = await LoadConfigFilesAsync(moldeFile);
                                if (moldeConfigEntities != null && moldeConfigEntities.Any())
                                {
                                    foreach (var moldeConfigEntity in moldeConfigEntities)
                                    {
                                        outcome.Add(moldeConfigEntity);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var firstModelConfigurationToAdd = await GetConfigEntityAsync(outcome, firstModelConfiguration);
                            outcome.Add(firstModelConfigurationToAdd);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Configuration file not found: {path}");
            }

            if (outcome != null && outcome.Count > 0)
            {
                var id = 1;
                foreach (var configuration in outcome)
                {
                    if (configuration != null)
                    {
                        configuration.Id = id++;
                    }
                }
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR LoadConfigFilesAsync");
            return default;
        }
    }

    #endregion Public functions

    #region Private functions
    public async Task<ConfigEntity> GetConfigEntityAsync(List<ConfigEntity> configEntities, ConfigFileEntity configFileEntity)
    {
        try
        {
            var outcome = default(ConfigEntity);

            if (configFileEntity != null)
            {
                outcome = new ConfigEntity
                {
                    Name = configFileEntity.Name,
                    Prompts = configFileEntity.Prompts,
                    Actions = configFileEntity.Actions
                };
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  FunctionName");
            return default;
        }
    }
    #endregion Private functions
}
