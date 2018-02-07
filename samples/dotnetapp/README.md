# .NET Core Development Sample

This .NET Core Docker sample demonstrates how to use Docker in your .NET Core development process. It builds multiple projects and executes unit tests in a container. The sample works with both Linux and Windows containers.

The [sample Dockerfile](Dockerfile) creates a .NET Core application Docker image based off of the [.NET Core Runtime Docker image](https://hub.docker.com/r/microsoft/dotnet/).

It uses the [Docker multi-stage build feature](https://github.com/dotnet/announcements/issues/18) to build the sample in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/r/microsoft/dotnet/). It builds and tests the samples and then copies the final build result into a Docker image based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/r/microsoft/dotnet/).

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker). You need the latest Windows 10 or Windows Server 2016 to use [Windows containers](http://aka.ms/windowscontainers). The instructions assume you have the [Git](https://git-scm.com/downloads) client installed.

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with git, using the following instructions.

```console
git clone https://github.com/dotnet/dotnet-docker-samples/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker-samples/archive/master.zip).

## Build and run the sample with Docker

You can build and run the sample in Docker using the following commands. The instructions assume that you are in the root of the repository.

```console
cd dotnetapp-dev
docker build -t dotnetapp-dev .
docker run --rm dotnetapp-dev Hello .NET Core from Docker
```

Note: The instructions above work for both Linux and Windows containers. The .NET Core docker images use [multi-arch tags](https://github.com/dotnet/announcements/issues/14), which abstract away different operating system choices for most use-cases.

## Run unit tests as part of `docker build`

The unit tests in this sample will run as part of the the `docker build` command listed above. You can make the unit test fail by changing the [unit test](tests/UnitTest1.cs) to match the test below. It is good to do this so that you can see the behavior of when tests fail as part of `docker build`.

```csharp
[Fact]
public void Test1()
{
    var inputString = "Dotnet-bot: Welcome to using .NET Core!";
    // var expectedString = "!eroC TEN. gnisu ot emocleW :tob-tentoD";
    var expectedString = "arbitrarily different string - won't match";
    var actualString = ReverseUtil.ReverseString(inputString);
    Assert.True(actualString == expectedString, "The input string was not reversed correctly.");
}
```

After changing the test, re-run `docker build` so that you can see the failure, with the following command.

```console
docker build -t dotnetapp-dev .
```

## Run unit tests as part of `docker run`

You can can also run the unit tests in the sample as part of `docker run`, with the primary benefit being that it is easier to harvest test logs. Running tests as part of `docker build` is useful as a means of getting early feedback, but it only really gives you pass/fail feedback since any useful information is primarily available solely via the console/terminal (not great for automation). The sample exposes a `testrunner` stage that you can build and then run explicity. This is why there are two `ENTRYPOINT` lines in the [Dockerfile](Dockerfile). You can then volume mount the appropriate directories in order to harvest test logs.

You can build and run the sample in Docker using the following commands. The instructions assume a location for the repo (please change to fit your environment).

First build an image, just to and including the `testrunner` stage.

```console
docker build --target testrunner -t dotnetapp-dev:test .
```

The following commands rely on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to enable the test runner to write test log files to your local drive. Without that, running tests as part of `docker run` isn't as useful.

You can run the sample on **Windows** using Windows containers using the following command.

```console
docker run --rm -v C:\git\dotnet-docker-samples\dotnetapp-dev\TestResults:C:\app\tests\TestResults dotnetapp-dev:test
```

You can run the sample on **Windows** using Linux containers using the following command. You should [enable shared drives](https://docs.docker.com/docker-for-windows/#shared-drives) first.

```console
docker run --rm -v C:\git\dotnet-docker-samples\dotnetapp-dev\TestResults:/app/tests/TestResults dotnetapp-dev:test
```

You can run the sample on **macOS** or **Linux** using the following command. You should enable  [file sharing](https://docs.docker.com/docker-for-mac/#file-sharing) first.

```console
docker run --rm -v "$(pwd)"/TestResults:/app/tests/TestResults dotnetapp-dev:test
```

You should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well that you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)

The unit testing in this Dockerfile demonstrates a couple approaches to unit testing with Docker. If you adopt this Dockerfile, you don't need to use both or either of these approaches. They are patterns that we considered useful for the unit testing use case.

## Build and run the sample locally

You can build and run the sample locally with the [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core) using the following instructions. The instructions assume that you are in the root of the repository.

```console
cd dotnetapp-dev
dotnet run Hello .NET Core
```

You can produce an application that is ready to deploy to production locally using the following command.

```console
dotnet publish -c release -o out
```

You can run the application on **Windows** using the following command.

```console
dotnet out\dotnetapp.dll
```

You can run the application on **Linux or macOS** using the following command.

```console
dotnet out/dotnetapp.dll
```

Note: The `-c release` argument builds the application in release mode (the default is debug mode). See the [dotnet run reference](https://docs.microsoft.com/dotnet/core/tools/dotnet-run) for more information on commandline parameters.

## Docker Images used in this sample

The following Docker images are used in this sample

* [microsoft/dotnet:2.0-sdk](https://hub.docker.com/r/microsoft/dotnet)
* [microsoft/dotnet:2.0-runtime](https://hub.docker.com/r/microsoft/dotnet)

## Related Resources

* [.NET Core Docker samples](../README.md)
* [.NET Framework Docker samples](https://github.com/Microsoft/dotnet-framework-docker-samples)
