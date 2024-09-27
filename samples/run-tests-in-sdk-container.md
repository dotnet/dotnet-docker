# Running Tests with Docker

You can use Docker to run your unit tests in a controlled environment using the [.NET SDK Docker image](../README.sdk.md).
Running tests in a container has several benefits:

- Tests are more insulated from dev and build machine configuration.
- Tests can run in an environment that more closely matches production - this is especially useful if your development and production environments don't match.
- Any additional setup needed for running tests can be configured in a Dockerfile.

There are multiple ways to run unit tests in containers, which are demonstrated in this document.

[Building in an SDK container](build-in-sdk-container.md) is a similar scenario and relies on similar patterns. [Building and testing multiple projects with Docker](complexapp/README.md) sample offers additional test patterns that you may want to adopt.

This document uses the [tests](complexapp/tests) that are part of [complexapp](complexapp). Some instructions assume that you have cloned this repo and are in the [complexapp](complexapp) directory.

## Run tests as part of Docker build

Docker [buildkit](https://docs.docker.com/reference/cli/docker/buildx/) has built-in support for [exporting files from the docker build command](https://docs.docker.com/build/building/export/).
We can use this to output test results directly to the disk when running a Docker build.

First validate which stages are available to build in the Dockerfile:

```console
docker build -q --call=targets 'https://github.com/dotnet/dotnet-docker.git#:samples/complexapp'

TARGET          DESCRIPTION
build           copies all project files and restores NuGet packages
test-build      builds the xUnit test project
test-run        runs the test project
test-output     is a from-scratch stage containing only test results
test-entrypoint exposes tests as the default executable for the stage
publish         builds and publishes complexapp
final (default) is the final runtime stage for running the app
```

If you're using Docker [buildkit](https://docs.docker.com/reference/cli/docker/buildx/), all of the extra test stages are skipped when targeting the `final (default)` stage, so there is no image size or compilation time cost to keep them in the same Dockerfile.

For this example, we want to run the `test-output` stage.
While we could run the `test-run` stage directly, Docker will automatically build that stage when we build the `test-output` stage since it depends on the test output files `test-run` anyways.

```pwsh
# Local build from the samples/complexapp directory
docker build --progress=plain --target "test-output" -o TestResults .

# Build without cloning this repo
docker build --progress=plain --target "test-output" -o TestResults 'https://github.com/dotnet/dotnet-docker.git#:samples/complexapp'
```

The `--progress=plain` argument is useful in testing scenarios because it ensures that all console output is captured and easily searchable.

The test results will be output directly in the `TestResults` directory on the host machine:

```xml
cat ./TestResults/TestResults.trx

<?xml version="1.0" encoding="utf-8"?>
<TestRun id="05544822-9565-4e2a-b910-6150a427aa32" name="@buildkitsandbox 2024-09-26 22:09:57" xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
...
```

The biggest downside to this approach is that in order to get the test output from the build, you must silence any non-zero exit codes output by the test run. For example:

```Dockerfile
RUN dotnet test --no-build --logger:trx || true
```

This means a failing test may not automatically your automated testing pipeline.
In this case, it's important to check the test output file for failure and use another mechanism to "fail" your build if necessary.

The following approach, running tests as an executable stage, works around that issue by building a test image and gather test outputs using a volume-mounted directory.

## Run tests in an executable stage

Tests can also be run as the `ENTRYPOINT` of a stage in your app's Dockerfile.
The [complexapp Dockerfile](./complexapp/Dockerfile) includes a `test-entrypoint` stage that demonstrates running via its `ENTRYPOINT`, as follows:

```Dockerfile
# test-entrypoint exposes tests as the default executable for the stage
FROM test-build AS test-entrypoint
ENTRYPOINT ["dotnet", "test", "--no-build", "--logger:trx"]
```

The following example demonstrates targeting the `test` stage with the `--target` argument, and with logging enabled, using PowerShell:

```pwsh
# Build the test image
PS> docker build --pull --target test-entrypoint -t complexapp-tests .
 => [internal] load build definition from Dockerfile
 ...
 => => naming to docker.io/library/complexapp-tests:latest
 => => unpacking to docker.io/library/complexapp-tests:latest


# Create output directory for test results
PS> mkdir TestResults


# Run the test image, mounting the TestResults directory into the container
PS> docker run --rm -v ${pwd}/TestResults:/source/tests/TestResults complexapp-tests
Test run for /source/tests/bin/Debug/net9.0/tests.dll (.NETCoreApp,Version=v9.0)
VSTest version 17.12.0-preview-24412-03 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_51029443fea7_2024-09-27_16_25_14.trx

Passed!  - Failed:     0, Passed:     3, Skipped:     0, Total:     3, Duration: 9 ms - tests.dll (net9.0)


# View the test results on the host machine
PS> ls TestResults

    Directory: C:\s\dotnet-docker\samples\complexapp\TestResults

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---           9/27/2024  9:25 AM           4962 _51029443fea7_2024-09-27_16_25_14.trx
```

## Run tests directly in a Docker container

The following examples demonstrate using `dotnet test` directly in a .NET SDK container.
It builds tests and dependent projects from source and then runs them.
You have to re-launch the container every time you want to test source code changes.

Alternatively, you can use `dotnet watch test`. This command reruns tests within a running container with every local code change.

### Requirements

The instructions assume that you have cloned the [repository](https://github.com/dotnet/dotnet-docker) locally.

You may need to enable [shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

Container scenarios that use volume mounting can produce conflicts between the `bin` and `obj` directories in local and container environments. To avoid that, you need to use a different set of `obj` and `bin` folders for your container environment. The easiest way to do that is to copy a custom [Directory.Build.props](Directory.Build.props) into the directory you are using (like the `complexapp` directory in the following example), either via copying from this repo or downloading with the following command:

```console
curl -o Directory.Build.props https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/Directory.Build.props
```

> [!NOTE]
> You may need to remove `bin` and `obj` directories if you run these instructions on Windows in both Windows and Linux container modes.

### Running the tests

You can run `dotnet test` within a .NET SDK container using the following pattern, with `docker run` and volume mounting. This initial example is demonstrated on Windows with PowerShell (in Linux container mode). Instructions for all OSes follow.

```console
> docker run --rm -v ${pwd}:/app -w /app/tests mcr.microsoft.com/dotnet/sdk:9.0 dotnet test --logger "trx;logfilename=TestResults.trx"
  Restore complete (6.3s)
  libfoo succeeded (0.4s) → /app/libfoo/bin/Debug/net9.0/libfoo.dll
  libbar succeeded (0.4s) → /app/libbar/bin/Debug/net9.0/libbar.dll
  tests succeeded (0.5s) → bin/Debug/net9.0/tests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.5.7+8f2703126a (64-bit .NET 9.0.0-rc.1.24431.7)
[xUnit.net 00:00:00.09]   Discovering: tests
[xUnit.net 00:00:00.12]   Discovered:  tests
[xUnit.net 00:00:00.12]   Starting:    tests
[xUnit.net 00:00:00.19]   Finished:    tests
Results File: /app/tests/TestResults/TestResults.trx
  tests test succeeded (1.2s)

Test summary: total: 3, failed: 0, succeeded: 3, skipped: 0, duration: 1.1s
Build succeeded in 9.4s
```

In this example, the tests (and any other required code) are [volume mounted](https://docs.docker.com/engine/admin/volumes/volumes/) into the container, and `dotnet test` is run from the `tests` directory (`-w` sets the working directory). Test results are written to disk with the `--logger:trx` flag.

When the `--logger:trx` flag is used, you should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open it in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well, which you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)

The following instructions demonstrate this scenario in various configurations with logging enabled.

### Linux or macOS

```console
docker run --rm -v $(pwd):/app -w /app/tests mcr.microsoft.com/dotnet/sdk:9.0 dotnet test --logger:trx
```

### Windows using Linux containers

This example uses PowerShell.

```console
docker run --rm -v ${pwd}:/app -w /app/tests mcr.microsoft.com/dotnet/sdk:9.0 dotnet test --logger:trx
```

### Windows using Windows containers

This example uses PowerShell.

```console
docker run --rm -v ${pwd}:C:\app -w C:\app\tests mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-ltsc2022 dotnet test --logger:trx
```

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
