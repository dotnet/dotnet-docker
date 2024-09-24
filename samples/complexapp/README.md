# Building and testing multiple projects with Docker

It is easy to build and test multiple projects with Docker by following some basic patterns. Docker comes with some requirements and some useful mechanisms that can help you manage various container-based workflows.

The `complexapp` sample is intended to act as a [very simple](complexapp/Program.cs) "complex application". It is composed of multiple projects, including a test project. It is used to demonstrate various workflows.

Simpler workflows are provided at [.NET Docker samples](../README.md).

The instructions assume that you have cloned this repo, have [Docker](https://www.docker.com/products/docker) installed, and have a command prompt open within the `samples/complexapp` directory within the repo.

## Building an image including multiple projects

The most common way to build images is using following pattern:

```console
docker build -t tag .
```

The most important aspect of that `docker` command is the `.` at the end. It represents the path to the build context, where Docker will look for assets that are referenced in the Dockerfile and for a `Dockerfile` (if not explicitly specified with the `-f` option). The build context is packaged up and then sent to the Docker daemon (the server), which performs the image build. It is required that all referenced assets are available within the build context, otherwise, they will not be available while the image is being built, which may produce the wrong image or produce an error. You can inspect intermediate or final images to validate that they contain the correct content if you see results you don't expect.

In the case of an application with multiple project dependencies, it may be intuitive to place the Dockerfile beside the project file for the application project, and to assign the build context to that same location. This will not work because project dependencies will not exist within the build context.

Instead, in a multi-project solution, the best pattern is to place the Dockerfile at the same location you would place a solution file, which is typically one directory above the application project. At that location, you can use the pattern  demonstrated above, using `.` for the build context at the same location as the Dockerfile, and enabling all resources to be naturally located within that context.

This is the approach used with [complexapp](.). The [Dockerfile](Dockerfile) for the sample is at the same location as the `.sln` file, and all projects are available when that same location is used as the build context. There are other options, but this approach is the easiest.

You can build and run the complexapp using the following commands:

```console
docker build --pull -t complexapp .
docker run --rm complexapp
```

It will restore and build all required projects and produce an application image.

One or more of the projects in your solution might be test projects. It can be useful to include testing within same container-based workflow as the application. This is helpful for at least two reasons:

* Integrating testing into this workflow will ensure that tests run in the same environment as the application.
* Get test feedback to validate images before publishing to a registry and deploying to production.

There are two primary ways to test within the workflow of an application container image:

* Run `dotnet test` as a `RUN` step within the image build.
* Expose an opt-in `ENTRYPOINT` as part of a Dockerfile stage.

This is different than running tests within a [.NET SDK container](../run-tests-in-sdk-container.md), which establishes a generic environment (which also works well). The rest of this document is focused on running tests within the same container environment as the application.

> [!NOTE]
> See [Establishing docker environment](../establishing-docker-environment.md) for more information on correctly configuring Dockerfiles and `docker build` commands.

## Running tests as an opt-in stage

There are multiple approaches for testing with containers, such as the `ENTRYPOINT` of an opt-in stage (covered in this section) or as part of `docker build` (covered in the next section). The opt-in test stage approach covered in this section is the recommended approach because it is more flexible.

The primary benefit of using an opt-in stage for testing is that it enables using the same environment as the build as an opt-in scenario and allows volume mounting (which isn't possible with `docker build`) to collect test logs.

The [Dockerfile](Dockerfile) includes a `test` stage that demonstrates running via its `ENTRYPOINT`, as follows.

```Dockerfile
# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test

COPY tests/*.csproj tests/
WORKDIR /source/tests
RUN dotnet restore

COPY tests/ .
RUN dotnet build --no-restore

ENTRYPOINT ["dotnet", "test", "--logger:trx", "--no-build"]
```

The presence of the `test` stage costs very little and doesn't significantly change the behavior of the build if you don't specifically target it. By default, the test stage `ENTRYPOINT` will not be used if you build this Dockerfile

The following example demonstrates targeting the `test` stage with the `--target` argument, and with logging enabled, using PowerShell:

```console
PS C:\git\dotnet-docker\samples\complexapp> docker build --pull --target test -t complexapp-test .
Sending build context to Docker daemon  12.81MB
Step 1/15 : FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
8.0: Pulling from dotnet/sdk
Successfully built f98c5453be3d
Successfully tagged complexapp-test:latest
SECURITY WARNING: You are building a Docker image from Windows against a non-Windows Docker host. All files and directories added to build context will have '-rwxr-xr-x' permissions. It is recommended to double check and reset permissions for sensitive files and directories.

PS C:\git\dotnet-docker\samples\complexapp> mkdir TestResults

PS C:\git\dotnet-docker\samples\complexapp> docker run --rm -v $pwd\TestResults:/source/tests/TestResults complexapp-test
  Determining projects to restore...
  Restored /source/tests/tests.csproj (in 7.73 sec).
  2 of 3 projects are up-to-date for restore.
  libbar -> /source/libbar/bin/Debug/netstandard2.0/libbar.dll
  libfoo -> /source/libfoo/bin/Debug/netstandard2.0/libfoo.dll
  tests -> /source/tests/bin/Debug/net8.0/tests.dll
Test run for /source/tests/bin/Debug/net8.0/tests.dll (.NETCoreApp,Version=v8.0)
Microsoft (R) Test Execution Command Line Tool Version 17.10.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_886d04dbf347_2020-11-02_18_30_59.trx

Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 2 ms - /source/tests/bin/Debug/net8.0/tests.dll (net8.0)

PS C:\git\dotnet-docker\samples\complexapp> dir .\TestResults\


    Directory: C:\git\dotnet-docker\samples\complexapp\TestResults

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a----         9/2/2021  12:31 PM           3583 _886d04dbf347_2021-09-02_18_30_59.trx
```

The following instructions demonstrate this scenario in various configurations, with logging enabled.

### Linux or macOS

```console
docker build --pull --target test -t complexapp-test .
docker run --rm -v ${pwd}/TestResults:/source/tests/TestResults complexapp-test
```

### Windows using Linux containers

The following example uses PowerShell.

```console
docker build --pull --target test -t complexapp-test .
docker run --rm -v ${pwd}\TestResults:/source/tests/TestResults complexapp-test
```

### Windows using Windows containers

The following example uses PowerShell.

```console
docker build --pull --target test -t complexapp-test .
docker run --rm -v ${pwd}\TestResults:c:\source\tests\TestResults complexapp-test
```

## Running tests while building an image

It is possible to run tests as part of `docker build`. This approach can be useful if you want `docker build` to fail if your tests fail. **This approach is not recommended**, as will be described later in this section.

This approach can be implemented by adding the following instructions to the `publish` stage. It is not included in the sample Dockerfile because it is not the recommended approach.

```Dockerfile
COPY tests/ /source/tests/
WORKDIR /source/tests
RUN dotnet restore
RUN dotnet test --no-restore --logger:trx
```

If you build the [sample Dockerfile](Dockerfile) with this change, you will see that tests are run during the build:

```console
> docker build --progress=plain -t complexapp .
#0 building with "desktop-linux" instance using docker driver

#1 [internal] load build definition from Dockerfile
#1 transferring dockerfile: 1.28kB done

...

#22 [publish 6/6] RUN dotnet test --no-restore --logger:trx
#22 1.108 /usr/share/dotnet/sdk/9.0.100-rc.1.24452.12/Sdks/Microsoft.NET.Sdk/targets/Microsoft.NET.RuntimeIdentifierInference.targets(326,5): message NETSDK1057: You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy [/source/tests/tests.csproj]
#22 1.748 /usr/share/dotnet/sdk/9.0.100-rc.1.24452.12/Sdks/Microsoft.NET.Sdk/targets/Microsoft.NET.RuntimeIdentifierInference.targets(326,5): message NETSDK1057: You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy [/source/libbar/libbar.csproj]
#22 4.167   libfoo -> /source/libfoo/bin/Debug/net9.0/libfoo.dll
#22 4.172   libbar -> /source/libbar/bin/Debug/net9.0/libbar.dll
#22 4.620   tests -> /source/tests/bin/Debug/net9.0/tests.dll
#22 4.642 Test run for /source/tests/bin/Debug/net9.0/tests.dll (.NETCoreApp,Version=v9.0)
#22 4.745 VSTest version 17.12.0-preview-24412-03 (x64)
#22 4.751
#22 4.858 Starting test execution, please wait...
#22 4.893 A total of 1 test files matched the specified pattern.
#22 5.615 Results File: /source/tests/TestResults/_buildkitsandbox_2024-09-05_17_14_28.trx
#22 5.616
#22 5.620 Passed!  - Failed:     0, Passed:     3, Skipped:     0, Total:     3, Duration: 7 ms - tests.dll (net9.0)
#22 DONE 5.7s

...
```

There are two main limitations to this approach:

* It's not possible to get the test logs out of the stage that was used to run the tests during the build.
* If tests fail, the image fails to build. This leaves you without a container image that you could otherwise use to further diagnose the problem that caused the tests to fail.

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/samples/README.md)
