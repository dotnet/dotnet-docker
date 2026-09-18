// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal partial record FromStagingPipelineOptions : CreatePullRequestOptions
{
    public const string StagingStorageAccountOptionName = "--staging-storage-account";
    public const string InternalOption = "--internal";

    /// <summary>
    /// A comma-delimited list of stage container names (e.g., "stage-1234567,stage-2345678")
    /// to use as a source for the update.
    /// </summary>
    public required string StageContainers { get; init; }

    /// <summary>
    /// Whether or not to use the internal versions of the staged build.
    /// </summary>
    public bool Internal { get; init; } = false;

    /// <summary>
    /// The mode in which to run the command.
    /// </summary>
    public ChangeMode Mode { get; init; } = ChangeMode.Local;

    /// <summary>
    /// This Azure Storage Account will be used as a source for the update.
    /// This should be one of two storage accounts: dotnetstagetest or dotnetstage.
    /// </summary>
    public string? StagingStorageAccount { get; init; }

    private static readonly Argument<string> s_stageContainers = new("stage-containers")
    {
        Description = "A comma-delimited list of stage container names to use as a source for the update (e.g., 'stage-1234567,stage-2345678')",
    };
    internal static readonly Option<string> StagingStorageAccountOption = new(StagingStorageAccountOptionName)
    {
        Description = "The Azure Storage Account to use as a source for the update."
            + " This should be one of two storage accounts: dotnetstagetest or dotnetstage."
            + " For example: https://dotnetstagetest.blob.core.windows.net/",
        DefaultValueFactory = _ => string.Empty,
    };
    private static readonly Option<bool> s_internal = new(InternalOption)
    {
        Description = "Whether or not to use the internal versions of the staged build. When not using an internal"
            + " build, Dockerfiles will be updated as if the staged build has already been released. When using an"
            + " internal build, Dockerfiles will be updated with internal download links and will only be buildable"
            + " by using an internal Azure DevOps access token.",
    };
    private static readonly Option<ChangeMode> s_mode = new("--mode")
    {
        Description = "The mode in which to run the command. Local mode makes changes directly to the local repo"
            + " without running any Git operations. Remote mode makes changes to a remote repo and submits a pull"
            + " request with the changes.",
        DefaultValueFactory = _ => ChangeMode.Local,
    };

    public static new void AddTo(Command command)
    {
        CreatePullRequestOptions.AddTo(command);
        command.Arguments.Add(s_stageContainers);
        command.Options.Add(StagingStorageAccountOption);
        command.Options.Add(s_internal);
        command.Options.Add(s_mode);
    }

    public static new FromStagingPipelineOptions Bind(ParseResult result) =>
        Bind(result, new FromStagingPipelineOptions
        {
            StageContainers = result.GetRequiredValue(s_stageContainers),
            StagingStorageAccount = result.GetRequiredValue(StagingStorageAccountOption),
            Internal = result.GetValue(s_internal),
            Mode = result.GetValue(s_mode),
        });

    /// <summary>
    /// Parses the comma-delimited list of stage containers and returns them as a list.
    /// </summary>
    public IReadOnlyList<string> GetStageContainerList()
    {
        return StageContainers
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}

internal static partial class StagingPipelineOptionsExtensions
{
    [GeneratedRegex(@"^stage-(\d+)$")]
    private static partial Regex StageContainerRegex { get; }

    /// <summary>
    /// Extracts the staging pipeline run ID from the stage container name.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if the stage container name is not in the expected format.
    /// </exception>
    public static int GetStagingPipelineRunId(string stageContainer)
    {
        var match = StageContainerRegex.Match(stageContainer);
        if (!match.Success)
        {
            throw new ArgumentException(
                $"Invalid stage container name '{stageContainer}'. Expected format: 'stage-{{buildId}}' (e.g., 'stage-1234567')");
        }
        return int.Parse(match.Groups[1].Value);
    }
}
