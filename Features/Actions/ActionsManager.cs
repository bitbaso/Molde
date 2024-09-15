using System.Diagnostics;
using System.Text.Json;
using Molde.Entities;
using Molde.Features.Actions.Interfaces;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;

namespace Molde.Features.Actions;

public class ActionsManager(ILogger<ActionsManager> logger) : IActionsManager
{

    /// <summary>
    /// Execute actions
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<bool> ExecuteActions(List<ActionEntity> actions, Dictionary<string, string> variables)
    {
        try
        {
            var outcome = default(bool);

            if (actions != null && actions.Count > 0)
            {
                foreach (var action in actions)
                {
                    if (action != null)
                    {
                        switch (action.Type)
                        {
                            case "add":
                                await AddFileAction(action, variables);
                                break;
                            case "modify":
                                await ModifyFileAction(action, variables);
                                break;
                            case "append":
                                await AppendToFileAction(action, variables);
                                break;
                            case "move":
                                await MoveFileAction(action);
                                break;
                            case "delete":
                                await DeleteFileAction(action);
                                break;
                            case "run":
                                await RunCommandAction(action);
                                break;
                            default:
                                Console.WriteLine($"Action type {action.Type} not recognized.");
                                break;
                        }
                    }
                }

                outcome = true;
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR ExecuteActions");
            return default;
        }
    }

    private async Task<bool> AddFileAction(ActionEntity action, Dictionary<string, string> variables)
    {
        try
        {
            var outcome = default(bool);

            if (action != null
                && variables != null)
            {
                var templateContent = await GetTemplateContent(action);

                if (!string.IsNullOrEmpty(templateContent))
                {
                    var template = Handlebars.Compile(templateContent);
                    if (template != null)
                    {
                        string result = template(variables);
                        File.WriteAllText(action.Output, result);
                        outcome = true;
                    }
                }
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AddFileAction");
            return default;
        }

    }

    private async Task<bool> ModifyFileAction(ActionEntity action, Dictionary<string, string> variables)
    {
        try
        {
            var outcome = default(bool);

            if (action != null
                && !string.IsNullOrEmpty(action.Template)
                && variables != null)
            {
                var templateContent = await GetTemplateContent(action);
                if (!string.IsNullOrEmpty(templateContent))
                {
                    var template = Handlebars.Compile(templateContent);
                    var contentToInsert = template(variables);

                    var lines = File.ReadAllLines(action.TargetFile);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(action.Marker))
                        {
                            lines[i] = lines[i].Replace(action.Marker, contentToInsert);
                            break;
                        }
                    }
                    File.WriteAllLines(action.TargetFile, lines);

                    outcome = true;
                }
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR ModifyFileAction");
            return default;
        }

    }

    private async Task<bool> AppendToFileAction(ActionEntity action, Dictionary<string, string> variables)
    {
        try
        {
            var outcome = default(bool);

            if (action != null
                && !string.IsNullOrEmpty(action.Template)
                && variables != null)
            {
                var templateContent = await GetTemplateContent(action);
                if (!string.IsNullOrEmpty(templateContent))
                {
                    var template = Handlebars.Compile(templateContent);
                    var contentToInsert = template(variables);

                    var lines = File.ReadAllLines(action.TargetFile);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(action.Marker))
                        {
                            int markerIndex = lines[i].IndexOf(action.Marker) + action.Marker.Length;
                            lines[i] = lines[i].Insert(markerIndex, contentToInsert);
                            break;
                        }
                    }
                    File.WriteAllLines(action.TargetFile, lines);

                    outcome = true;
                }
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AppendToFileAction");
            return default;
        }
    }

    private async Task<bool> MoveFileAction(ActionEntity action)
    {
        try
        {
            var outcome = default(bool);

            if (action != null
                && !string.IsNullOrEmpty(action.Source)
                && !string.IsNullOrEmpty(action.Destination))
            {
                if (File.Exists(action.Source))
                {
                    File.Move(action.Source, action.Destination);
                }
                else
                {
                    Console.WriteLine($"File exists: {action.Source}");
                }

                outcome = true;
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR MoveFileAction");
            return default;
        }
    }

    private async Task<bool> DeleteFileAction(ActionEntity action)
    {
        try
        {
            var outcome = default(bool);

            if (action != null && !string.IsNullOrEmpty(action.Path))
            {
                if (File.Exists(action.Path))
                {
                    File.Delete(action.Path);
                }

                outcome = true;
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR DeleteFileAction");
            return default;
        }
    }

    private async Task<bool> RunCommandAction(ActionEntity action)
    {
        try
        {
            var outcome = default(bool);

            if (action != null)
            {
                var processStartInfo = new ProcessStartInfo("cmd", $"/c {action.Command}");
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;

                using (var process = Process.Start(processStartInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        Console.WriteLine(result);
                    }
                }

                outcome = true;
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR RunCommandAction");
            return default;
        }
    }

    /// <summary>
    /// Gets template content from action
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private async Task<string> GetTemplateContent(ActionEntity action)
    {
        try
        {
            var outcome = default(string);

            if (action != null)
            {

                if (!string.IsNullOrEmpty(action.Template))
                {
                    outcome = action.Template;
                }
                else if (!string.IsNullOrEmpty(action.TemplateFile))
                {
                    outcome = File.ReadAllText(action.TemplateFile);
                }
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR GetTemplateContent");
            return default;
        }
    }
}
