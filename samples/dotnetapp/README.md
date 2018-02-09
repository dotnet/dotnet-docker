# .NET Core Docker Sample

This sample demonstrates how to use .NET Core and Docker together. It builds multiple projects and executes unit tests in a container. The sample works with both Linux and Windows containers and can also be used without Docker.

The sample builds the application in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/r/microsoft/dotnet/). It builds and tests the application and then copies the final build result into a Docker image based on the smaller [.NET Core Docker Runtime image](https://hub.docker.com/r/microsoft/dotnet/). It uses Docker [multi-stage build](https://github.com/dotnet/announcements/issues/18) and [multi-arch tags](https://github.com/dotnet/announcements/issues/14) where appropriate.

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker).

Multiple variations of this sample have been provided:

* [Multi-arch sample with build and unit testing](Dockerfile)
* [Multi-arch basic sample](Dockerfile.basic)
* [Alpine sample with build and unit testing](Dockerfile.alpine)
* [Alpine basic sample, enables Globalization](Dockerfile.alpinewithglobalization)
* [ARM32 sample with build and unit testing](Dockerfile.arm32)

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions.

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Build and run the sample with Docker

You can build and run the sample in Docker using the following commands. The instructions assume that you are in the root of the repository.

```console
cd dotnetapp
docker build -t dotnetapp .
docker run --rm dotnetapp Hello .NET Core from Docker
```

You can build a basic [Dockerfile](Dockerfile.basic) that does not include testing.

## Build and run the sample for Alpine

You can build and run the sample with [alpine](https://hub.docker.com/_/alpine/) using the following commands. The instructions assume that you are in the root of the repository.

```console
cd dotnetapp
docker build -t dotnetapp -f Dockerfile.alpine .
docker run --rm dotnetapp Hello .NET Core from Alpine
```

[Globalization is disabled](https://github.com/dotnet/announcements/issues/20) by default with Alpine images in order to produce smaller container images. You can re-enable globalization if your application relies on it. You can build a basic Alpine [Dockerfile](Dockerfile.alpinewithglobalization) that enables globalization.

> Related: See [.NET Core Alpine Docker Image announcement](https://github.com/dotnet/dotnet-docker-nightly/issues/500)

## Build and run the sample for Raspberry Pi

You can build and run the sample for Raspberry Pi using the following commands. The instructions assume that you are in the root of the repository.

```console
cd dotnetapp-prod
docker build -t dotnetapp -f Dockerfile.arm32 .
```

Push the image to a registry and pull from the Raspberry Pi with the following command. You need to be signed into the Docker client to `docker push` to Docker Hub.

```console
docker run --rm mydockername/dotnetapp-prod-arm32 Hello .NET Core from Docker
```

## Running unit tests

You can run unit tests with this sample, with either `docker build` or `docker run`.

### Run unit tests as part of `docker build`

The unit tests in this sample will run as part of the the `docker build` command listed above. You can make the unit test fail by changing the [unit test](tests/UnitTest1.cs) to match the test below. It is good to do this so that you can see the behavior of when tests fail as part of `docker build`.

```csharp
[Fact]
public void ReverseString()
{
    var inputString = "The quick brown fox jumps over the lazy dog";
    var expectedString = "Not the expected string.";
    var returnedString = StringUtils.ReverseString(inputString);
    Assert.True(expectedString == returnedString, "The input string was not reversed correctly.");
}
```

After changing the test, re-run `docker build` so that you can see the failure, with the following command.

```console
docker build -t dotnetapp .
```

You can instead build an Alpine-based image using the following command.

```console
docker build -t dotnetapp -f Dockerfile.alpine .
```

### Run unit tests as part of `docker run`

You can can also run the unit tests in the sample as part of `docker run`, with the primary benefit being that it is easier to harvest test logs. Running tests as part of `docker build` is useful as a means of getting early feedback, but it only really gives you pass/fail feedback since any useful information is primarily available solely via the console/terminal (not great for automation). The sample exposes a `testrunner` stage that you can build and then run explicity. This is why there are two `ENTRYPOINT` lines in the [Dockerfile](Dockerfile). You can then volume mount the appropriate directories in order to harvest test logs.

You can build and run the sample in Docker using the following commands. The instructions assume a location for the repo (please change to fit your environment).

First build an image, just to and including the `testrunner` stage.

```console
docker build --target testrunner -t dotnetapp:test .
```

You can instead build an Alpine-based image using the following command.

```console
docker build --target testrunner -t dotnetapp -f Dockerfile.alpine .
```

The following commands rely on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to enable the test runner to write test log files to your local drive. Without that, running tests as part of `docker run` isn't as useful.

You can run the sample on **Windows** using Windows containers using the following command.

```console
docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:C:\app\tests\TestResults dotnetapp:test
```

You can run the sample on **Windows** using Linux containers using the following command. You should [enable shared drives](https://docs.docker.com/docker-for-windows/#shared-drives) first.

```console
docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:/app/tests/TestResults dotnetapp:test
```

You can run the sample on **macOS** or **Linux** using the following command. You should enable  [file sharing](https://docs.docker.com/docker-for-mac/#file-sharing) first.

```console
docker run --rm -v "$(pwd)"/TestResults:/app/tests/TestResults dotnetapp:test
```

You should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well that you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)

The unit testing in this Dockerfile demonstrates a couple approaches to unit testing with Docker. If you adopt this Dockerfile, you don't need to use both or either of these approaches. They are patterns that we considered useful for the unit testing use case.

## Build and run the sample locally

You can build and run the sample locally with the [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core) using the following instructions. The instructions assume that you are in the root of the repository.

```console
cd dotnetapp
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

* [microsoft/dotnet:sdk](https://hub.docker.com/r/microsoft/dotnet)
* [microsoft/dotnet:2.0-runtime](https://hub.docker.com/r/microsoft/dotnet)

## Related Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Core Docker](../../README.md)
* [.NET Framework Docker samples](https://github.com/Microsoft/dotnet-framework-docker-samples)
