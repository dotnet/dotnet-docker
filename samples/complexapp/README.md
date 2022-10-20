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

> Note: See [Establishing docker environment](../establishing-docker-environment.md) for more information on correctly configuring Dockerfiles and `docker build` commands.

## Running tests as an opt-in stage

There are multiple approaches for testing with containers, such as the `ENTRYPOINT` of an opt-in stage (covered in this section) or as part of `docker build` (covered in the next section). The opt-in test stage approach covered in this section is the recommended approach because it is more flexible.

The primary benefit of using an opt-in stage for testing is that it enables using the same environment as the build as an opt-in scenario and allows volume mounting (which isn't possible with `docker build`) to collect test logs.

The [Dockerfile](Dockerfile) includes a `test` stage that demonstrates running via its `ENTRYPOINT`, as follows.

```Dockerfile
# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test
WORKDIR /source/tests
COPY tests/ .
ENTRYPOINT ["dotnet", "test", "--logger:trx"]
```

The presence of the `test` stage costs very little and doesn't significantly change the behavior of the build if you don't specifically target it. By default, the test stage `ENTRYPOINT` will not be used if you build this Dockerfile

The following example demonstrates targeting the `test` stage with the `--target` argument, and with logging enabled, using PowerShell:

```console
PS C:\git\dotnet-docker\samples\complexapp> docker build --pull --target test -t complexapp-test .
Sending build context to Docker daemon  12.81MB
Step 1/15 : FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
7.0: Pulling from dotnet/sdk
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
  tests -> /source/tests/bin/Debug/net7.0/tests.dll
Test run for /source/tests/bin/Debug/net7.0/tests.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.0.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_886d04dbf347_2020-11-02_18_30_59.trx

Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 2 ms - /source/tests/bin/Debug/net7.0/tests.dll (net7.0)

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

It is possible to run tests as part of `docker build`. This approach can be useful if you want `docker build` to fail if your tests fail. It is not generally recommended, as will be described later in this section.

This approach can be implemented by the following pattern. It is not included in the sample Dockerfile because it is not the recommended approach.

```Dockerfile
WORKDIR /source/tests
RUN dotnet restore
RUN dotnet test --no-restore --logger:trx
```

The following example demonstrates this pattern, as if it was part of [Dockerfile](Dockerfile). You would then be able run tests using the normal `docker build` pattern, as follows:

```console
docker build --pull -t complexapp .
```

You will see that tests are run while building the image, as you can see in the following example (many build steps have been cut out to make the log easier to read).

```console
> docker build --pull -t complexapp .
Sending build context to Docker daemon  12.79MB
Step 1/24 : FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
7.0: Pulling from dotnet/sdk
Digest: sha256:02606610ffd96978da91adeac1b73af9ac536b85a3034b061d0c7d8d9fcd6790
Status: Image is up to date for mcr.microsoft.com/dotnet/sdk:7.0
 ---> 9817c25953a8
Step 17/24 : RUN dotnet test --logger:trx
 ---> Running in 4678e2e6456d
  Determining projects to restore...
  Restored /source/tests/tests.csproj (in 7.73 sec).
  2 of 3 projects are up-to-date for restore.
  libbar -> /source/libbar/bin/Debug/netstandard2.0/libbar.dll
  libfoo -> /source/libfoo/bin/Debug/netstandard2.0/libfoo.dll
  tests -> /source/tests/bin/Debug/net7.0/tests.dll
Test run for /source/tests/bin/Debug/net7.0/tests.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.0.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_886d04dbf347_2021-09-02_18_30_59.trx

Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 2 ms - /source/tests/bin/Debug/net7.0/tests.dll (net7.0)
Removing intermediate container 4678e2e6456d
 ---> 6deeeacdaaf2
Step 24/24 : ENTRYPOINT ["dotnet","complexapp.dll"]
 ---> Using cache
 ---> b1584b78d34a
Successfully built b1584b78d34a
Successfully tagged complexapp:latest
SECURITY WARNING: You are building a Docker image from Windows against a non-Windows Docker host. All files and directories added to build context will have '-rwxr-xr-x' permissions. It is recommended to double check and reset permissions for sensitive files and directories.
```

After building the container image, it is still possible to get the logs from the intermediate container image in which the tests were run. The intermediate stage container image is listed as `6deeeacdaaf2` in the console output. Using that information, we can boot up the container image and copy the log files from it, using the following pattern:

```console
C:\git\dotnet-docker\samples\complexapp>docker images | findstr 6deeeacdaaf2
<none>                                        <none>                    6deeeacdaaf2        About a minute ago   910MB
C:\git\dotnet-docker\samples\complexapp>docker run --rm -dit --entrypoint tail --name complexapp-test 6deeeacdaaf2
cb97ba418f4ceed93eab06527cbd0dfcd2c41cc2fc2dd0e6e0eed99215a6e786
C:\git\dotnet-docker\samples\complexapp>docker cp complexapp-test:/source/tests/TestResults .
C:\git\dotnet-docker\samples\complexapp>dir TestResults
 Volume in drive C is Windows
 Volume Serial Number is 384B-0B6E

 Directory of C:\git\dotnet-docker\samples\complexapp\TestResults

09/02/2021  02:41 PM    <DIR>          .
09/02/2021  02:41 PM    <DIR>          ..
09/02/2021  02:41 PM             3,635 _4678e2e6456d_2021-09-19_22_41_08.trx
```

There are two problems with this approach. It is cumbersome and if tests fail, it is not possible to copy the logs from the intermediate container layer, since that layer won't exist. This limitation, and the difficulty of copying files out of intermediate layers, demonstrates the weakness of this approach.

Alternatively, you can build with the `--rm=false` option. This leaves the intermediate build containers and you can then issue the `docker cp` command to retrieve the results. The use of docker system prune makes it easy to cleanup the intermediate build containers.

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/samples/README.md)
