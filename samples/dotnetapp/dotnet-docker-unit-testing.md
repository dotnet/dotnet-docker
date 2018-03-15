# Running .NET Core Unit Tests with Docker

You can run .NET Core unit tests in Docker with either `docker build` or `docker run`.

Running tests via `docker build` is useful as a means of getting early feedback, primarily pass/fail results printed via the console/terminal.

Running tests via `docker run` is useful as a means of getting complete test results captured with volume mounting.

These instructions are based on the [.NET Core Docker Sample](README.md).

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with [git](https://git-scm.com/downloads), using the following instructions.

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Run unit tests as part of `docker build`

You can run [unit tests](tests) as part of `docker build`, using the following commands. Running tests in this way is useful to get pass/fail results for building Docker images. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
```

You can make the unit test fail by changing the [unit test](tests/UnitTest1.cs) to match the following test. It is good to do make tests fail so that you can see the behavior when that happens as part of `docker build`.

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

After changing the test, rerun `docker build` so that you can see the failure, with the following command.

```console
docker build -t dotnetapp .
```

### Run unit tests as part of `docker run`

You can run [unit tests](tests) as part of `docker run` using the following commands. Running tests in this way is useful to get complete tests results for Docker images. The [sample Dockerfile](Dockerfile) exposes multiple [Dockerfile stages](https://docs.docker.com/engine/reference/commandline/build/#specifying-target-build-stage-target) that you can separately target as part of `docker build` and run. The sample Dockerfile includes a `testrunner` stage with a separate `ENTRYPOINT` for unit testing, which is required to maintain a single Dockerfile.

The following commands rely on [volume mounting](https://docs.docker.com/engine/admin/volumes/volumes/) (that's the `-v` argument in the following commands) to enable the test runner to write test log files to your local drive. Without that, running tests as part of `docker run` isn't as useful. You may need to [Enable shared drives (Windows)](https://docs.docker.com/docker-for-windows/#shared-drives) or [file sharing (macOS)](https://docs.docker.com/docker-for-mac/#file-sharing) first.

#### Build the testrunner stage

The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull --target testrunner -t dotnetapp:test .
```

If you want to test with Alpine Linux, you can alternatively build with [Dockerfile.alpine-x64](Dockerfile.alpine-x64) with the following command.

```console
docker build --pull --target testrunner -t dotnetapp -f Dockerfile.alpine-x64 .
```

#### Run the testrunner stage

Use the following commands, given your environment:

**Windows** using **Linux containers**

```console
docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:/app/tests/TestResults dotnetapp:test
```

**Linux or macOS** using **Linux containers**

```console
docker run --rm -v "$(pwd)"/TestResults:/app/tests/TestResults dotnetapp:test
```

**Windows** using **Windows containers**

```console
docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:C:\app\tests\TestResults dotnetapp:test
```

#### Reading the Results

You should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well that you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)
