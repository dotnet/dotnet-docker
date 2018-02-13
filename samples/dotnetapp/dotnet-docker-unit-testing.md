# Running .NET Core Unit Tests with Docker

You can run unit tests in Docker with either `docker build` or `docker run`. These instructions are based on the [.NET Core Docker Sample](README.md).

## Run unit tests as part of `docker build`

This [sample](Dockerfile) runs [unit tests](tests) as part of `docker build`, using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build -t dotnetapp .
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

You can instead build an Alpine-based image using the following command.

```console
docker build -t dotnetapp -f Dockerfile.alpine .
```

### Run unit tests as part of `docker run`

You can run the unit tests as part of `docker run`. The primary benefit this approach is that it is easier to harvest test logs as part of `docker run`. Running tests as part of `docker build` is useful as a means of getting early feedback, but it only really gives you pass/fail feedback since any useful information is primarily available solely via the console/terminal (can be a problem for automation).

The [sample](Dockerfile) exposes a `testrunner` stage that you can build and then run. This option explains why there are two `ENTRYPOINT` lines in the [Dockerfile](Dockerfile). When you run the image, you can volume mount the appropriate directories in order to harvest test logs.

You can build and run the sample in Docker using the following commands. The instructions assume that you are in the root of the repository.

First build an image, just to and including the `testrunner` stage.

```console
cd samples
cd dotnetapp
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

You can run the sample on **Windows** using Linux containers using the following command. [Enable shared drives](https://docs.docker.com/docker-for-windows/#shared-drives) first.

```console
docker run --rm -v C:\git\dotnet-docker\samples\dotnetapp\TestResults:/app/tests/TestResults dotnetapp:test
```

You can run the sample on **macOS** or **Linux** using the following command. Enable [file sharing](https://docs.docker.com/docker-for-mac/#file-sharing) first.

```console
docker run --rm -v "$(pwd)"/TestResults:/app/tests/TestResults dotnetapp:test
```

You should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well that you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)

The unit testing in this [Dockerfile](Dockerfile) demonstrates a couple approaches to unit testing with Docker. If you adopt this Dockerfile, you don't need to use both or either of these approaches. They are patterns that we consider useful for the unit testing use case.
