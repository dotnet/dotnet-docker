# Contributing

See [dotnet/runtime Contributing](https://github.com/dotnet/runtime/blob/main/CONTRIBUTING.md) for information about coding styles, source structure, making pull requests, and more.

## General Feedback and Questions

Please open a [discussion](https://github.com/dotnet/dotnet-docker/discussions).

## Reporting Security Issues

Please keep in mind that the GitHub issue tracker is intended for reporting **non-security** bugs and feature requests.

If you're reporting the presence of a disclosed security vulnerability, such as a CVE reported in one of our container images, please follow our documented [guidance on vulnerability reporting](https://github.com/dotnet/dotnet-docker/blob/main/documentation/vulnerability-reporting.md).

If you believe you have an issue that affects the security of .NET, please do NOT create an issue and instead email your issue details to <secure@microsoft.com>.
Your report may be eligible for our [bug bounty](https://www.microsoft.com/msrc/bounty-dot-net-core), but ONLY if it is reported through email.

## Bugs and Feature Requests

Before reporting a new issue, try to find an existing issue if one already exists.
If it already exists, please upvote it (👍) or add a comment to the issue with your unique scenario and requirements.
Upvotes and clear details on the issue's impact help us prioritize issues to work on.
If you can't find an existing issue, file a new one - we would rather get duplicate feedback than none.

We aspire to respond to all GitHub issues within 48 hours (first contact).
We triage issues and decide which issues to prioritize on a weekly basis, so if you don't receive a reply within a week or two, please feel free to add another reply to your own issue or mention the @dotnet/dotnet-docker-reviewers team.

## How to Submit a PR

### Before you write code

Please consider opening a feature request.
We are happy to accept community contributions - however, until we discuss your specifc ideas and features as a team, we can't guarantee that we will accept all community PRs.
The last thing we want is for you to spend time on an implementation before the team and community agree on the proposed change.
Once the community and the .NET team are in agreement on a proposal, we are happy to work on the feature ourselves.
However, if you'd still like to implement it yourself, you can request the issue be assigned to you.

### Branches

When making PRs, all source code changes (e.g. Dockerfiles, tests, and infrastructure) should be made in the [nightly branch](https://github.com/dotnet/dotnet-docker/tree/nightly). Only changes to the samples and documentation will be accepted against the [main branch](https://github.com/dotnet/dotnet-docker/tree/main).

### Building

The [`build-and-test.ps1`](https://github.com/dotnet/dotnet-docker/blob/main/build-and-test.ps1) script will build and test the .NET Docker images. Given the matrix of supported .NET versions, distros, and architectures there are numerous Dockerfiles and building can take a while. To make this manageable, the script supports several options for filtering down what images get built and tested.

- Build and test all of the .NET 9.0 images for the Docker platform your machine is targeting (e.g. linux/x64, linux/arm, linux/arm64, windows/x64).

    ``` console
    > ./build-and-test.ps1 -Version 9.0
    ```

- Build the 9.0 Ubuntu Noble images using version and OS arguments. Note that this *will not* build `noble-chiseled` images as those are labeled by a different `OS` in [manifest.json](/manifest.json).

    ``` console
    > ./build-and-test.ps1 -Version 9.0 -OS noble -Mode Build
    ```

- Build the 9.0 Ubuntu Noble images using Dockerfile paths. This *will* will build `noble-chiseled` images as those Dockerfiles will match the `noble` part of the `Paths` argument.

    ``` console
    > ./build-and-test.ps1 -Paths '*9.0*noble*' -Mode Build
    ```

- Build and test the samples

    ``` console
    > ./build-and-test.ps1 -Paths '*samples*' -TestCategories sample
    ```

- Test the 9.0 Nano Server 1809 images (remember to switch to Windows container mode in Docker Desktop)

    ``` console
    > ./build-and-test.ps1 -Version 9.0 -OS nanoserver-1809 -Mode Test
    ```

### Editing Dockerfiles

The [Dockerfiles](https://github.com/search?q=repo%3Adotnet%2Fdotnet-docker+path%3Asrc%2F**%2FDockerfile&type=code&ref=advsearch) contained in this repo are generated from a set of [Cottle](https://cottle.readthedocs.io/en/stable/page/01-overview.html) based [templates](https://github.com/dotnet/dotnet-docker/tree/main/eng/dockerfile-templates). A single template generates the set of Dockerfiles that are similar (e.g. all Windows sdk Dockerfiles for a particular .NET version). This ensures consistency across the various Dockerfiles and eases the burden of making changes to the Dockerfiles. Instead of editing the Dockerfiles directly, the templates should be updated and then the Dockerfiles should get regenerated by running the [generate Dockerfiles script](https://github.com/dotnet/dotnet-docker/blob/main/eng/dockerfile-templates/Get-GeneratedDockerfiles.ps1).

#### Dockerfile style guide

Use the following formatting guidelines when authoring Dockerfiles in this repo:

- Separate instructions with an empty newline.
- Separate stages with two empty newlines.
- Prefer to use long-form command line options for better readability.
- Leave an empty newline at the end of the Dockerfile.

### Editing READMEs

The [READMEs](https://github.com/search?q=repo%3Adotnet%2Fdotnet-docker+path%3A**%2FREADME*+-path%3Aeng+-path%3Asamples&type=code&ref=advsearch&p=1) contained in this repo are used as the descriptions for the Docker repositories the images are published to.  Just like the Dockerfiles, the READMEs are generated from a set of [Cottle](https://cottle.readthedocs.io/en/stable/page/01-overview.html) based [templates](https://github.com/dotnet/dotnet-docker/tree/main/eng/readme-templates).  This ensures consistency across the various READMEs and eases the burden of making changes.  Instead of editing the READMEs directly, the templates should be updated and then the READMEs should get regenerated by running the [generate READMEs script](https://github.com/dotnet/dotnet-docker/blob/main/eng/readme-templates/Get-GeneratedReadmes.ps1).

### Tests

There are several types of [tests](https://github.com/dotnet/dotnet-docker/tree/main/tests) in this repo.

1. Image tests
    - Unit tests that validate the static state of images, based on their filesystem contents or manifest/image config.
      This includes things like verifying which environment variables are defined and which packages are installed.
    - Scenario tests that run images to validate basic user scenarios.
      For example, use the SDK image to create, build and run a .NET app.
1. `"pre-build"` tests
    - Validate that tags adhere to a specific set of rules ([`StaticTagTests.cs`](tests/Microsoft.DotNet.Docker.Tests/StaticTagTests.cs))
    - Verify the state of generated Dockerfile templates (public and internal versions)

When editing Dockerfiles, please ensure the appropriate test changes are also made.

#### Debugging Tests Using VS Code

This repo comes with VS Code Task and Launch Profiles to help you debug tests.

To start, open [tasks.json](.vscode/tasks.json) and find the "Test with debugger" task.
Check the `args` and `env` settings to filter down to the exact image you want to test.

To filter tests to a specific image, use the `DOCKERFILE_PATHS` environment variable.
For example, to test only Alpine 3.21 ASP.NET images, you could set the `DOCKERFILE_PATHS` to `src/aspnet/9.0/alpine3.21/amd64`.
To run specific individual tests, you can use [Xunit test filtering](https://learn.microsoft.com/dotnet/core/testing/selective-unit-tests?pivots=xunit) arguments to filter by fully qualified test name and test category.

To start debugging, open the VS Code command palette and type "Tasks: Run Task", then choose the "Test with debugger" task. The terminal will open and print a process ID:

```console
Host debugging is enabled. Please attach debugger to testhost process to continue.
Process Id: 19972, Name: testhost
Waiting for debugger attach...
```

From the "Run and Debug" sidebar panel, run the "Attach .NET Debugger" launch configuration (once it's selected, you can quickly run it again by pressing F5).
VS Code will prompt you for a process ID to attach to.
Type in the PID that was printed to the terminal earlier.
Now, VS Code is attached to the .NET Debugger.
Press F5 (Continue) to start test execution.

#### Verifying Internal Dockerfiles

Internal Dockerfiles are validated using "snapshot" testing, which uses tooling to record and test the accepted state of the Dockerfiles.
If your changes fail tests due to changes in the internal Dockerfiles, you will need to review the changes before the tests can pass.
You can use a local dotnet tool to accept or reject the changes.

1. Run the failing test(s). For example: `./tests/run-tests.ps1 -Paths "*" -TestCategories "pre-build" -CustomTestFilter "VerifyInternalDockerfilesOutput"`
1. The failing test will output updated baseline files in the `tests/Microsoft.DotNet.Docker.Tests/Baselines/` directory, ending in `*.received.txt`.
1. Accept or discard the changes: `./tests/accept-changes.ps1 [-Discard]`. This script will either rename all of the `.received.txt` files to `.approved.txt` or remove them.
1. If the git diff of the changes look acceptable, then commit the changes.

### Metadata Changes

The [`manifest.json`](https://github.com/dotnet/dotnet-docker/blob/main/manifest.json) contains metadata used by the engineering infrastructure to build and publish the images.  It includes information such as:

- Dockerfiles to build
- Image Tags
- Manifest/shared tags to create and which images they reference
- Docker repositories to publish the images to
- Dockerfile templates used to generate the Dockerfiles
- etc.

When adding or removing Dockerfiles, it is important to update the `manifest.json` accordingly.

### Updating Product Versions

Updating the product versions (e.g. .NET runtime, ASP.NET runtime, PowerShell, etc.) contained within the images is typically performed by automation. All of the product version information is stored in the [`manifest.versions.json`](https://github.com/dotnet/dotnet-docker/blob/main/manifest.versions.json) file. The Dockerfile templates reference the product versions numbers and checksums from this file. Updating a product version involves updating the `manifest.versions.json` and regenerating the Dockerfiles. If there are cases where you need to update a product version, you can use the [update-dependencies](https://github.com/dotnet/dotnet-docker/tree/main/eng/update-dependencies) tool.  The tool will do the following:

1. Update the product versions and checksums stored in `manifest.versions.json`
1. Regenerate the Dockerfiles
1. Update the tags listing in the readmes

The following examples illustrate how to run `update-dependencies`:

- Update the 9.0 product versions (uses a helper script for running update-dependencies)

    ``` console
    > ./eng/Set-DotnetVersions.ps1 -ProductVersion 9.0 -SdkVersion 9.0.100 -RuntimeVersion 9.0.0 -AspnetVersion 9.0.0
    ```

- Update the .NET Monitor version (uses a helper script for running update-dependencies)

    ``` console
    > ./eng/Set-DotnetVersions.ps1 -ProductVersion 8.0 -MonitorVersion 8.0.5
    ```

- Update the PowerShell version used in the 9.0 images

    ``` console
    > dotnet run --project .\eng\update-dependencies\ -- specific 9.0 --product-version powershell=7.5.0
    ```

#### Checking Markdown links locally

This repo uses [UmbrellaDocs/linkspector](https://github.com/UmbrellaDocs/linkspector)
to automatically validate links in markdown files. You can run this tool
locally using Docker.

1. Build linkspector Docker image:
   `docker build --no-cache --pull --build-arg LINKSPECTOR_PACKAGE= -t umbrelladocs/linkspector https://github.com/UmbrellaDocs/linkspector.git`
2. Run linkspector:
   `docker run --rm -it -v ${PWD}:/app umbrelladocs/linkspector bash -c 'linkspector check -c /app/.github/linters/.linkspector.yml'`
