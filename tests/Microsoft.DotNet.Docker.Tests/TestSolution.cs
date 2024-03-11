#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.DotNet.Docker.Tests;

public class TestSolution : IDisposable
{
    private bool _disposed = false;
    private readonly DockerHelper _dockerHelper;
    private readonly ProductImageData _imageData;
    private readonly string _appName = "app";
    private readonly string _solutionDir;
    private readonly string _testProjectDir;
    private readonly string _appProjectDir;

    public string SampleName { get; }

    public string SolutionDir => _solutionDir;

    public TestSolution(ProductImageData imageData, string sampleName, DockerHelper dockerHelper, bool excludeTests = false)
    {
        SampleName = sampleName;
        _imageData = imageData;
        _dockerHelper = dockerHelper;

        _solutionDir = Path.Combine(Directory.GetCurrentDirectory(), $"{sampleName}App{DateTime.Now.ToFileTime()}");
        _testProjectDir = Path.Combine(_solutionDir, "tests");
        _appProjectDir = Path.Combine(_solutionDir, _appName);

        CreateTestSolutionWithSdkImage(_solutionDir, sampleName);

        if (!excludeTests)
        {
            InjectCustomTestCode(_appProjectDir);
        }
    }

    private string CreateTestSolutionWithSdkImage(string solutionDir, string appType)
    {
        Directory.CreateDirectory(solutionDir);
        string appProjectContainerName = _imageData.GetIdentifier($"create-{appType}");
        string testProjectContainerName = _imageData.GetIdentifier("create-test");

        try
        {
            CreateProjectWithSdkImage(appType, Path.Combine(solutionDir, "app"), appProjectContainerName);

            CreateProjectWithSdkImage("xunit", _testProjectDir, testProjectContainerName);
            File.Copy(Path.Combine(DockerHelper.TestArtifactsDir, "UnitTests.cs"), Path.Combine(_testProjectDir, "UnitTests.cs"));

            string sourceDockerfileName = $"Dockerfile.{DockerHelper.DockerOS.ToLower()}";

            File.Copy(
                Path.Combine(DockerHelper.TestArtifactsDir, sourceDockerfileName),
                Path.Combine(solutionDir, "Dockerfile"));

            string nuGetConfigFileName = "NuGet.config";
            if (Config.IsNightlyRepo)
            {
                nuGetConfigFileName += ".nightly";
            }

            File.Copy(Path.Combine(DockerHelper.TestArtifactsDir, nuGetConfigFileName), Path.Combine(solutionDir, "NuGet.config"));
            File.Copy(Path.Combine(DockerHelper.TestArtifactsDir, ".dockerignore"), Path.Combine(solutionDir, ".dockerignore"));
        }
        catch (Exception)
        {
            if (Directory.Exists(solutionDir))
            {
                Directory.Delete(solutionDir, recursive: true);
            }

            throw;
        }
        finally
        {
            _dockerHelper.DeleteContainer(appProjectContainerName);
            _dockerHelper.DeleteContainer(testProjectContainerName);
        }

        return solutionDir;
    }

    private void InjectCustomTestCode(string appDir)
    {
        string programFilePath = Path.Combine(appDir, "Program.cs");

        SyntaxTree programTree = CSharpSyntaxTree.ParseText(File.ReadAllText(programFilePath));

        string newContent;

        MethodDeclarationSyntax? mainMethod = programTree.GetRoot().DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(method => method.Identifier.ValueText == "Main");

        if (mainMethod is null || mainMethod.Body is null)
        {
            // Handles project templates that use top-level statements instead of a Main method
            IEnumerable<SyntaxNode> nodes = programTree.GetRoot().ChildNodes();

            IEnumerable<UsingDirectiveSyntax> usingDirectives = nodes.OfType<UsingDirectiveSyntax>();

            IEnumerable<SyntaxNode> otherNodes = nodes.Except(usingDirectives);

            StringBuilder builder = new();
            foreach (UsingDirectiveSyntax usingDir in usingDirectives)
            {
                builder.Append(usingDir.ToFullString());
            }

            // Verify a web request succeeds
            builder.AppendLine("System.Console.WriteLine(\"Verifying a web request succeeds\");");
            builder.AppendLine("var response = await new System.Net.Http.HttpClient().GetAsync(\"https://www.microsoft.com\");");
            builder.AppendLine("response.EnsureSuccessStatusCode();");

            // Verify write access is allowed to the user directory
            builder.AppendLine(GetUserDirectoryWriteAccessValidationCode());

            foreach (SyntaxNode otherNode in otherNodes)
            {
                builder.Append(otherNode.ToFullString());
            }

            newContent = builder.ToString();
        }
        else
        {
            StatementSyntax testHttpsConnectivityStatement = SyntaxFactory.ParseStatement(
                // Verify a web request succeeds
                "System.Console.WriteLine(\"Verifying a web request succeeds\");" +
                "var task = new System.Net.Http.HttpClient().GetAsync(\"https://www.microsoft.com\");" +
                "task.Wait();" +
                "task.Result.EnsureSuccessStatusCode();" +
                // Verify write access is allowed to the user directory
                GetUserDirectoryWriteAccessValidationCode());

            MethodDeclarationSyntax newMainMethod = mainMethod.InsertNodesBefore(
                mainMethod.Body.ChildNodes().First(),
                new SyntaxNode[] { testHttpsConnectivityStatement });

            SyntaxNode newRoot = programTree.GetRoot().ReplaceNode(mainMethod, newMainMethod);
            newContent = newRoot.ToFullString();
        }

        File.WriteAllText(programFilePath, newContent);
    }

    private string GetUserDirectoryWriteAccessValidationCode()
    {
        if (_imageData.IsDistroless && _imageData.Version.Major == 6 && _imageData.OS.StartsWith(OS.Mariner))
        {
            return string.Empty;
        }

        string userDirEnvVarName = DockerHelper.IsLinuxContainerModeEnabled ? "HOME" : "USERPROFILE";
        return
            "System.Console.WriteLine(\"Verifying write access to user directory\");" +
            $"System.IO.File.WriteAllText(System.Environment.GetEnvironmentVariable(\"{userDirEnvVarName}\") + \"/test.txt\", \"test\");";
    }

    private void CreateProjectWithSdkImage(string templateName, string destinationPath, string containerName)
    {
        IEnumerable<string> args =
        [
            templateName,
            $"--framework net{_imageData.Version}",
            "--no-restore"
        ];

        if (templateName.Contains("web"))
        {
            args = args.Append("--exclude-launch-settings");
        }

        string projectContainerDir = $"/{_appName}";

        _dockerHelper.Run(
            image: _imageData.GetImage(DotNetImageRepo.SDK, _dockerHelper),
            name: containerName,
            command: $"dotnet new {string.Join(' ', args)}",
            workdir: projectContainerDir,
            optionalRunArgs: $"--platform {_imageData.Platform}",
            skipAutoCleanup: true);

        _dockerHelper.Copy($"{containerName}:{projectContainerDir}", destinationPath);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Directory.Delete(_solutionDir, recursive: true);
            }

            _disposed = true;
        }
    }
}
