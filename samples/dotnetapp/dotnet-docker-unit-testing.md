# Running .NET Core Unit Tests with Docker

The testing scenario showcases the value of Docker since testing is more valuable when the test environment has high fidelity with target environments. Imagine you support your application on multiple operating systems or operating system versions. You can test your application in each of them within Docker. It is easy to do and incredibly valuable.

This document is focussed on unit testing as part of container image building. For unit testing as part of development, see [Develop .NET Core Applications in a Container](dotnet-docker-dev-in-container.md).

These instructions are based on the [.NET Core Docker Sample](README.md).

## Try a pre-built Unit Testing Script

You can quickly try unit testing in a container with a pre-built [build script](build.ps1). The instructions assume that you are at the root of the repo:

Type the following commands on Windows:

```console
cd samples
cd dotnetapp
build.cmd
```

Type the following commands on macOS or Linux:

```console
cd samples
cd dotnetapp
./build.sh
```

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions.

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Best Practice for Testing with Docker

The most obvious choice is to perform unit testing within a `Dockerfile`, like in the following Dockerfile fragment:

```Dockerfile
WORKDIR /app/tests
COPY tests .
RUN dotnet test
```

Running tests via `docker build` is useful as a means of getting early feedback, primarily with pass/fail results printed to the console/terminal. This model works OK for testing but doesn't scale well for two reasons:

* `docker build` will fail if there are errors, which are inherent to testing.
* `docker build` doesn't allow volume mounting, which is required to collect test logs.

Testing with `docker run` is a great alternative, since it doesn't suffer from either of these two challenges. Testing with `docker build` is only really useful if you want your build to fail if tests fail. The instructions in this document show you how to test with `docker run`.

## Building Test Runner Image

The [sample Dockerfile](Dockerfile) exposes multiple [Dockerfile stages](https://docs.docker.com/engine/reference/commandline/build/#specifying-target-build-stage-target) that you can separately target with `docker build`. The sample Dockerfile includes a `testrunner` stage with a separate `ENTRYPOINT` for unit testing. We want to build to that stage so that we can run the tests.

The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull --target testrunner -t dotnetapp:test .
```

If you want to test with Alpine Linux, you can alternatively build with [Dockerfile.alpine-x64](Dockerfile.alpine-x64) with the following command.

```console
docker build --pull --target testrunner -t dotnetapp:test -f Dockerfile.alpine-x64 .
```

## Running a Test Runner Container

The following commands rely on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument) to enable the test runner to write test log files to your local drive. You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

Use the following commands to run tests, for your specific environment:

### Windows using Linux containers

```console
mkdir TestResults
docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:/app/tests/TestResults dotnetapp:test
```

### Linux or macOS using Linux containers

```console
mkdir TestResults
docker run --rm -v "$(pwd)"/TestResults:/app/tests/TestResults dotnetapp:test
```

### Windows using Windows containers

```console
mkdir TestResults
docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:C:\app\tests\TestResults dotnetapp:test
```

### Fail the test

You can make the unit test fail by changing the [unit test](tests/UnitTest1.cs) to match the following test. It is useful to observe both passing and failing test cases.

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

After changing the test, rerun the instructions (including `docker build`) so that you can see the failure.

### Reading the Results

You should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well that you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
