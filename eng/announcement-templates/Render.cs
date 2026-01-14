#!/usr/bin/env dotnet
#:package Fluid.Core@2.31.0
#:package Spectre.Console@0.54.1-alpha.0.26
#:package System.CommandLine@2.0.2

using System.CommandLine;
using Fluid;
using Spectre.Console;

var renderCommand = new RenderTemplateCommand(templateFileInfo =>
{
    if (!templateFileInfo.Exists)
    {
        Console.Error.WriteLine($"Template file not found: {templateFileInfo.FullName}");
        return 1;
    }

    if (!TemplateDefinitions.TemplateIsSupported(templateFileInfo))
    {
        Console.Error.WriteLine($"Unsupported template file: {templateFileInfo.Name}");
        return 1;
    }

    // Parse the template first, so any errors are caught before prompting for input
    var template = TemplateDefinitions.ParseTemplate(templateFileInfo, out var parseError);
    if (template is null)
    {
        Console.Error.WriteLine($"Failed to parse template: {parseError}");
        return 1;
    }

    // Display some helpful reference information right before prompting the user for input.
    DisplayPatchTuesdayReferenceText();

    // This will prompt the user for the template parameters.
    TemplateContext context = TemplateDefinitions.GetTemplateContext(templateFileInfo);

    // Finally, render the template with the provided parameters.
    var result = template.Render(context);

    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule("[green]Generated Announcement[/]"));
    AnsiConsole.WriteLine();
    Console.WriteLine(result);

    return 0;
});

var parseResult = renderCommand.Parse(args);
return parseResult.Invoke();


static void DisplayPatchTuesdayReferenceText()
{
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine("[grey]Patch Tuesdays Reference:[/]");
    for (int i = -4; i <= 4; i++)
    {
        var pt = DateOnly.GetPatchTuesday(i);
        var label = i == 0 ? "this month" : i.ToString("+0;-0");
        AnsiConsole.MarkupLine($"[grey]  {label,11}: {pt:yyyy-MM-dd}[/]");
    }

    AnsiConsole.WriteLine();
}

static class TemplateDefinitions
{
    // All supported templates and their associated context factories.
    private static readonly Dictionary<string, Func<TemplateContext>> s_templateContexts = new()
    {
        ["alpine-floating-tag-update.md"] = FloatingTagTemplateParameters.ContextFactory,
    };

    public static TemplateContext GetTemplateContext(FileInfo templateFileInfo)
    {
        var contextFactory = s_templateContexts[templateFileInfo.Name];
        var templateContext = contextFactory();
        return templateContext;
    }

    public static IFluidTemplate? ParseTemplate(FileInfo templateFile, out string? error)
    {
        var parser = new FluidParser();
        var templateText = File.ReadAllText(templateFile.FullName);

        if (!parser.TryParse(templateText, out var template, out string? internalError))
        {
            error = internalError;
            return null;
        }

        error = null;
        return template;
    }

    public static bool TemplateIsSupported(FileInfo templateFile) =>
        s_templateContexts.ContainsKey(templateFile.Name);
}

sealed class RenderTemplateCommand : RootCommand
{
    public RenderTemplateCommand(Func<FileInfo, int> handler) : base("Render announcement template")
    {
        var templateFileArgument = new Argument<FileInfo>("templateFile")
        {
            Description = "The template file to read and display on the console",
        };
        Arguments.Add(templateFileArgument);

        SetAction(parseResult =>
        {
            var templateFileResult = parseResult.GetValue(templateFileArgument);
            if (parseResult.Errors.Count == 0 && templateFileResult is FileInfo validTemplateFile)
            {
                return handler(validTemplateFile);
            }

            if (parseResult.Errors.Count > 0)
            {
                foreach (var error in parseResult.Errors)
                    Console.Error.WriteLine(error.Message);

                return 1;
            }

            // Show help text
            Parse("-h").Invoke();

            return 0;
        });
    }
}

sealed record FloatingTagTemplateParameters(
    string NewVersion,
    string OldVersion,
    DateTime PublishDate,
    DateTime ReleaseDate,
    DateTime EolDate,
    string PublishDiscussionUrl,
    string DotnetExampleVersion)
{
    public static Func<TemplateContext> ContextFactory { get; } = () =>
    {
        var model = PromptForInput();
        return new TemplateContext(model);
    };

    public static FloatingTagTemplateParameters PromptForInput()
    {
        var newVersion = AnsiConsole.Prompt(
            new TextPrompt<string>("New Alpine version:")
                .DefaultValue("3.XX"));

        var oldVersion = AnsiConsole.Prompt(
            new TextPrompt<string>("Previous Alpine version:")
                .DefaultValue("3.XX"));

        var publishDate = AnsiConsole.Prompt(
            new TextPrompt<DateOnly>($"When was Alpine {newVersion} published?")
                .DefaultValue(DateOnly.GetPatchTuesday(-1)));

        const string DiscussionQueryLink = "https://github.com/dotnet/dotnet-docker/discussions/categories/announcements?discussions_q=is%3Aopen+category%3AAnnouncements+alpine";
        var publishDiscussionUrl = AnsiConsole.Prompt(
            new TextPrompt<string>($"Link to announcement for publishing Alpine {newVersion} images (see {DiscussionQueryLink}):"));

        var releaseDate = AnsiConsole.Prompt(
            new TextPrompt<DateOnly>($"When were floating tags moved from Alpine {oldVersion} to {newVersion}?")
                .DefaultValue(DateOnly.GetPatchTuesday(0)));

        var eolDate = AnsiConsole.Prompt(
            new TextPrompt<DateOnly>($"When will we stop publishing Alpine {oldVersion} images?")
                .DefaultValue(DateOnly.GetPatchTuesday(3)));

        var dotnetExampleVersion = AnsiConsole.Prompt(
            new TextPrompt<string>(".NET example version for tags:")
                .DefaultValue("10.0"));

        return new FloatingTagTemplateParameters(
            newVersion,
            oldVersion,
            publishDate.ToDateTime(TimeOnly.MinValue),
            releaseDate.ToDateTime(TimeOnly.MinValue),
            eolDate.ToDateTime(TimeOnly.MinValue),
            publishDiscussionUrl,
            dotnetExampleVersion);
    }
}

internal static class DateOnlyExtensions
{
    extension(DateOnly date)
    {
        /// <summary>
        /// Gets the Patch Tuesday (second Tuesday of the month) for a month
        /// relative to the current month.
        /// </summary>
        /// <param name="offset">
        /// The number of months from the current month.
        /// 0 = this month, 1 = next month, -3 = three months ago.
        /// </param>
        /// <returns>The date of Patch Tuesday for the target month.</returns>
        public static DateOnly GetPatchTuesday(int offset = 0)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var targetMonth = today.AddMonths(offset);
            var firstOfMonth = new DateOnly(targetMonth.Year, targetMonth.Month, 1);

            // Find the first Tuesday
            var daysUntilTuesday = ((int)DayOfWeek.Tuesday - (int)firstOfMonth.DayOfWeek + 7) % 7;
            var firstTuesday = firstOfMonth.AddDays(daysUntilTuesday);

            // Second Tuesday is 7 days later
            return firstTuesday.AddDays(7);
        }
    }
}
