# Running Tests with Docker

You can use Docker to run your unit tests in an isolated environment using the [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). This is useful if your development and production environments don't match, like, for example, Windows and Linux, respectively. There are multiple ways to run unit tests in containers, which are demonstrated in this document.

[Building in an SDK container](build-in-sdk-container.md) is a similar scenario and relies on similar patterns. [Building and testing multiple projects with Docker](https://github.com/dotnet/dotnet-docker/blob/samples/samples/complexapp/README.md) sample offers additional test patterns that you may want to adopt.

This document uses the [tests](complexapp/tests) that are part of [complexapp](complexapp). The instructions assume that you are in the [complexapp](complexapp) directory.

The following examples demonstrate using `dotnet test` in a .NET Core SDK container. It builds tests and dependent projects from source and then runs them. You have to re-launch the container every time you want to test source code changes.

Alternatively, you can use `dotnet watch test`. This command reruns tests within a running container, with every local code change.

## Requirements

The instructions assume that you have cloned the [repository](https://github.com/dotnet/dotnet-docker) locally.

You may need to enable [shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

Container scenarios that use volume mounting can produce conflicts between the `bin` and `obj` directories in local and container environments.  To avoid that, you need to use a different set of `obj` and `bin` folders for your container environment. The easiest way to do that is to copy a custom [Directory.Build.props](Directory.Build.props) into the directory you are using (like the `complexapp` directory in the following example), either via copying from this repo or downloading with the following command:

```console
curl -o Directory.Build.props https://raw.githubusercontent.com/dotnet/dotnet-docker/master/samples/Directory.Build.props
```

> Note: You may need to remove `bin` and `obj` directories if you run these instructions on Windows in both Windows and Linux container modes.

## Running tests

The easiest approach is to run `dotnet test` within a .NET Core SDK container using the following pattern, with `docker run` and volume mounting.  This initial example is demonstrated on Windows with PowerShell (in Linux container mode). Instructions for all OSes follow.

```console
> docker run --rm -v ${pwd}:/app -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet test
Test run for /app/tests/bin/Debug/netcoreapp3.1/tests.dll(.NETCoreApp,Version=v3.1)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 1.5825 Seconds
 ```

In this example, the tests (and any other required code) are [volume mounted](https://docs.docker.com/engine/admin/volumes/volumes/) into the countainer, and `dotnet test` is run from the `tests` directory (`-w` sets the working directory). Test results can be read from the console or from logs, which can be written to disk with the `--logger:trx` flag.

When the `--logger:trx` flag is used, you should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well that you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)

The following instructions demonstrate this scenario in various configurations, with logging enabled.

### Linux or macOS

```console
docker run --rm -v $(pwd):/app -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet test --logger:trx
```

### Windows using Linux containers

This example uses PowerShell.

```console
docker run --rm -v ${pwd}:/app -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet test --logger:trx
```

### Windows using Windows containers

This example uses PowerShell.

```console
docker run --rm -v ${pwd}:C:\app -w C:\app\tests mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet test --logger:trx
```

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
