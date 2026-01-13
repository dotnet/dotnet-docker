#!/usr/bin/env dotnet
#:package Fluid.Core@2.31.0
#:package Spectre.Console@0.54.1-alpha.0.26

using Fluid;
using Spectre.Console;

DisplayPatchTuesdayReferenceText();

var templatePath = "alpine-floating-tag-update.md";

var parser = new FluidParser();
var templateText = File.ReadAllText(templatePath);
if (!parser.TryParse(templateText, out var template, out var error))
{
    Console.Error.WriteLine($"Error parsing template: {error}");
    return 1;
}

var newVersion = AnsiConsole.Prompt(
    new TextPrompt<string>("New Alpine version:")
        .DefaultValue("3.XX"));

var oldVersion = AnsiConsole.Prompt(
    new TextPrompt<string>("Previous Alpine version:")
        .DefaultValue("3.XX"));

var publishDate = AnsiConsole.Prompt(
    new TextPrompt<DateOnly>($"When was Alpine {newVersion} published?")
        .DefaultValue(GetPatchTuesday(-1)));

const string DiscussionQueryLink = "https://github.com/dotnet/dotnet-docker/discussions/categories/announcements?discussions_q=is%3Aopen+category%3AAnnouncements+alpine";
var publishDiscussionUrl = AnsiConsole.Prompt(
    new TextPrompt<string>($"Link to announcement for publishing Alpine {newVersion} images (see {DiscussionQueryLink}):"));

var releaseDate = AnsiConsole.Prompt(
    new TextPrompt<DateOnly>($"When were floating tags moved from Alpine {oldVersion} to {newVersion}?")
        .DefaultValue(GetPatchTuesday(0)));

var eolDate = AnsiConsole.Prompt(
    new TextPrompt<DateOnly>($"When will we stop publishing Alpine {oldVersion} images?")
        .DefaultValue(GetPatchTuesday(3)));

var dotnetExampleVersion = AnsiConsole.Prompt(
    new TextPrompt<string>(".NET example version for tags:")
        .DefaultValue("10.0"));

var context = new TemplateContext(new
{
    new_version = newVersion,
    old_version = oldVersion,
    publish_date = publishDate.ToDateTime(TimeOnly.MinValue),
    release_date = releaseDate.ToDateTime(TimeOnly.MinValue),
    eol_date = eolDate.ToDateTime(TimeOnly.MinValue),
    publish_discussion_number = publishDiscussionUrl,
    dotnet_example_version = dotnetExampleVersion
});

var result = await template.RenderAsync(context);

AnsiConsole.WriteLine();
AnsiConsole.Write(new Rule("[green]Generated Announcement[/]"));
AnsiConsole.WriteLine();
Console.WriteLine(result);

return 0;

static void DisplayPatchTuesdayReferenceText()
{
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine("[grey]Patch Tuesdays Reference:[/]");

    for (int i = -4; i <= 4; i++)
    {
        var pt = GetPatchTuesday(i);
        var label = i == 0 ? "this month" : i.ToString("+0;-0");
        AnsiConsole.MarkupLine($"[grey]  {label,11}: {pt:yyyy-MM-dd}[/]");
    }

    AnsiConsole.WriteLine();
}

/// <summary>
/// Gets the Patch Tuesday (second Tuesday of the month) for a month relative
/// to the current month.
/// </summary>
/// <param name="offset">
/// The number of months from the current month.
/// 0 = this month, 1 = next month, -3 = three months ago.
/// </param>
/// <returns>The date of Patch Tuesday for the target month.</returns>
static DateOnly GetPatchTuesday(int offset = 0)
{
    var today = DateTime.Today;
    var targetMonth = today.AddMonths(offset);
    var firstOfMonth = new DateOnly(targetMonth.Year, targetMonth.Month, 1);

    // Find the first Tuesday
    var daysUntilTuesday = ((int)DayOfWeek.Tuesday - (int)firstOfMonth.DayOfWeek + 7) % 7;
    var firstTuesday = firstOfMonth.AddDays(daysUntilTuesday);

    // Second Tuesday is 7 days later
    return firstTuesday.AddDays(7);
}
