// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

/// <summary>
/// Runs repository generators in the workspace being updated, failing if a script exits unsuccessfully.
/// </summary>
internal static class ScriptRunner
{
    public static Task GenerateDockerfilesAsync(string repoRoot, CancellationToken cancellationToken = default)
    {
        string scriptPath = Path.Combine(repoRoot, "eng", "dockerfile-templates", "Get-GeneratedDockerfiles.ps1");
        return RunAsync(scriptPath, repoRoot, cancellationToken);
    }

    public static Task GenerateReadmesAsync(string repoRoot, CancellationToken cancellationToken = default)
    {
        string scriptPath = Path.Combine(repoRoot, "eng", "readme-templates", "Get-GeneratedReadmes.ps1");
        return RunAsync(scriptPath, repoRoot, cancellationToken);
    }

    private static async Task RunAsync(string scriptPath, string repoRoot, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Trace.TraceInformation($"Executing '{scriptPath}'");

        var startInfo = new ProcessStartInfo("pwsh")
        {
            WorkingDirectory = repoRoot,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("-NoProfile");
        startInfo.ArgumentList.Add("-File");
        startInfo.ArgumentList.Add(scriptPath);
        using var process = new Process { StartInfo = startInfo };

        // Support both execution within Windows 10, Nano Server and Linux environments.
        try
        {
            process.Start();
        }
        catch (Win32Exception e) when (e.NativeErrorCode == 2)
        {
            startInfo.FileName = "powershell";
            process.Start();
        }

        try
        {
            await process.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // The publisher must not clean up its workspace while a generator still writes to it.
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }

            await process.WaitForExitAsync(CancellationToken.None);
            throw;
        }

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Script '{scriptPath}' exited with code {process.ExitCode}.");
        }
    }
}
