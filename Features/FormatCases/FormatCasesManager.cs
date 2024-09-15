using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Molde.Entities;
using Molde.Features.Actions.Interfaces;
using Molde.Features.FormatCases.Interfaces;
using HandlebarsDotNet;
using System.Reflection.Metadata.Ecma335;

namespace Molde.Features.FormatCases;

public class FormatCasesManager : IFormatCasesManager
{

    #region Public functions
    /// <summary>
    /// Register format cases
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<bool> RegisterHelpers()
    {
        try
        {
            var outcome = default(bool);

            Handlebars.RegisterHelper("camelCase", (writer, context, parameters) =>
                            writer.WriteSafeString(ToCamelCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("snakeCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToSnakeCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("kebabCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToKebabCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("dotCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToDotCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("pathCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToPathCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("pascalCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToPascalCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("lowerCase", (writer, context, parameters) =>
                writer.WriteSafeString(parameters[0].ToString().ToLower()));

            Handlebars.RegisterHelper("sentenceCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToSentenceCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("constantCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToConstantCase(parameters[0].ToString())));

            Handlebars.RegisterHelper("titleCase", (writer, context, parameters) =>
                writer.WriteSafeString(ToTitleCase(parameters[0].ToString())));

            outcome = true;


            return outcome;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    #endregion Public functions

    #region Private functions
    private string ToCamelCase(string input) => ToLowerCaseFirst(ToPascalCase(input));

    private string ToSnakeCase(string input) => ToDelimitedCase(input, '_');

    private string ToKebabCase(string input) => ToDelimitedCase(input, '-');

    private string ToDotCase(string input) => ToDelimitedCase(input, '.');

    private string ToPathCase(string input) => ToDelimitedCase(input, '/');

    private string ToPascalCase(string input) => Regex.Replace(input, @"(^\w|_\w)", m => m.Value.Replace("_", "").ToUpper());

    private string ToLowerCaseFirst(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToLower(input[0]) + input.Substring(1);
    }

    private string ToDelimitedCase(string input, char delimiter)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]) && i > 0 && input[i - 1] != delimiter)
            {
                sb.Append(delimiter);
            }
            sb.Append(char.ToLower(input[i]));
        }

        return sb.ToString().Replace(" ", delimiter.ToString());
    }

    private string ToSentenceCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        input = input.ToLower();
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    private string ToConstantCase(string input) => ToDelimitedCase(input, '_').ToUpper();

    private string ToTitleCase(string input)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }

    #endregion Private functions
}
