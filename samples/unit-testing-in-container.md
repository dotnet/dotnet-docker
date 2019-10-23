# Running .NET Core Unit Tests with Docker

You can use Docker to run your unit tests in an isolated environment using the [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). This is useful if your development and production environments don't match, like, for example, Windows and Linux, respectively. There are a few ways to run unit tests in containers, which are demonstrated in this document.

[Containerized build](containerized-build.md) is a similar scenario and relies on similar patterns.

This document uses the [tests](complexapp/tests) that are part of [complexapp](complexapp). The instructions assume that you are in the [complexapp](complexapp) directory.

## Running tests using the .NET Core SDK container image

The easiest approach is to run `dotnet test` within a .NET Core SDK container using the following pattern, with `docker run` and volume mounting:

```console
C:\git\dotnet-docker\samples\complexapp>docker run --rm -v %cd%:/app -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet test
Test run for /app/tests/bin/Debug/netcoreapp3.0/tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.1.1
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 1.9393 Seconds
 ```

In this example, the tests (and any other required code) are [volume mounted](https://docs.docker.com/engine/admin/volumes/volumes/) into the countainer, and `dotnet test` is run from the `tests` directory (`-w` sets the working directory). Test results can be read from the console or from logs, which can be written to disk with the `--logger:trx` flag.

You should find a `.trx` file in the TestResults folder. You can open this file in Visual Studio to see the results of the test run, as you can see in the following image. You can open in Visual Studio (File -> Open -> File) or double-click on the TRX file (if you have Visual Studio installed). There are other TRX file viewers available as well that you can search for.

![Visual Studio Test Results](https://user-images.githubusercontent.com/2608468/35361940-2f5ab914-0118-11e8-9c40-4f252f4568f0.png)

The following instructions demonstrate this scenario in various configurations, with logging enabled.

### Linux or macOS

```console
MacBook-Pro:complexapp rich$ docker run --rm -v $(pwd):/app -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet test --logger:trx
```

### Windows using Linux containers

```console
C:\git\dotnet-docker\samples\complexapp>docker run --rm -v %cd%:/app -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet test --logger:trx
```

### Windows using Windows containers

```console
C:\git\dotnet-docker\samples\complexapp>docker run --rm -v %cd%:\app -w \app\tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet test --logger:trx
```

## Test your application in a container while you develop

You can test your application in a container with every local code change. This approach is useful if you have your IDE and a command prompt open at the same time, with the latter showing the console output for `dotnet watch test`.

This approach uses a similar pattern, with the .NET Core SDK container image, `docker run`, volume mounting and a file watcher:

```console
rich@MacBook-Pro complexapp % docker run --rm -it -v ~/git/dotnet-docker/samples/complexapp:/app/ -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch test
watch : Polling file watcher is enabled
watch : Started
Test run for /app/tests/bin/Debug/netcoreapp3.0/tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.
                                                                                
Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 1.5602 Seconds
watch : Exited
watch : Waiting for a file to change before restarting dotnet...
```

You can test this working by simply editing [UnitTest1.cs](complexapp/tests/UnitTest1.cs), such as changing the input or expected strings. You should a test failure within a few seconds.

The following instructions demonstrates this scenario with various configurations.

### Linux or macOS

```console
docker run --rm -it -v ~/git/dotnet-docker/samples/complexapp:/app/ -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch test
```

### Windows using Linux containers

```console
docker run --rm -it -v c:\git\dotnet-docker\samples\complexapp:/app/ -w /app/tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch test
```

### Windows using Windows containers

```console
docker run --rm -it -v c:\git\dotnet-docker\samples\complexapp:c:\app\ -w \app\tests mcr.microsoft.com/dotnet/core/sdk:3.0 dotnet watch test
```

The commands above log test results to the console. You can additionally log results as a TRX file by appending `--logger:trx` to the previous test commands, specifically `dotnet watch test --logger:trx`. TRX logging is also demonstrated in [Running .NET Core Unit Tests with Docker](dotnet-docker-unit-testing.md).


## Running tests as an opt-in docker stage

It is possible to run tests as the `ENTRYPOINT` to an opt-in build stage. If you run tests as part of `docker build` (covered later in this document), your build may fail, and you may not want that.

The [Dockerfile](complexapp/Dockerfile) includes a `test` stage that demonstates this pattern.

```Dockerfile
# test app
FROM build AS test
WORKDIR /source/tests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]
```

By default, if you build this Dockerfile, you will get a working app in a container, and no tests will be run. The `test` stage costs very little by being present if you don't specifically target it. In the default case, the `ENTRYPOINT` that is added by the `test` stage is overwritten by the final `ENTRYPOINT` in the Dockerfile.

You can try that using the following commands:

```console
docker build --pull -t complexapp .
docker run --rm complexapp
```

The primary win of using an opt-in stage is that it enables testing using the same environment as the build and allows volume mounting (which isn't possible with `docker build`) to collect test logs.

The following example demonstrates targeting the `test` stage with the `--target` argument, and with logging enabled: 

```console
MacBook-Pro:complexapp rich$ docker build --pull --target test -t complexapp-test .
MacBook-Pro:complexapp rich$ docker run --rm -v $(pwd)/TestResults:/source/tests/TestResults complexapp-test
Test run for /source/tests/bin/Debug/netcoreapp3.0/tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_fb9d4f0d1b60_2019-10-21_23_08_55.trx

Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 0.9126 Seconds
MacBook-Pro:complexapp rich$ ls TestResults/
_643bdbd70901_2019-10-21_23_19_34.trx
```

The following instructions demonstrate this scenario in various configurations, with logging enabled.

### Linux or macOS

```console
MacBook-Pro:complexapp rich$ docker build --pull --target test -t complexapp-test .
MacBook-Pro:complexapp rich$ docker run --rm -v $(pwd)/TestResults:/source/tests/TestResults complexapp-test
```

### Windows using Linux containers

```console
C:\git\dotnet-docker\samples\complexapp>docker build --pull --target test -t complexapp-test .
C:\git\dotnet-docker\samples\complexapp>docker run --rm -v %cd%/TestResults:/source/tests/TestResults complexapp-test
```

### Windows using Windows containers

```console
C:\git\dotnet-docker\samples\complexapp>docker build --pull --target test -t complexapp-test .
C:\git\dotnet-docker\samples\complexapp>docker run --rm -v %cd%/TestResults:c:\source\tests\TestResults complexapp-test
```

## Running tests as part of docker build

It is possible to run tests as part of docker build. This approach can makes sense if you want to achieve the following characteristics:

- Fail `docker build` if tests fail.
- Run tests in the same environment as the build.

The [Dockerfile](complexapp/Dockerfile) includes two different models for testing (one of which is commented out) as part of the `test` stage, as follows. In this example, the `RUN` line is uncommented and will be used when we build the Dockerfile. Instead, the `ENTRYPOINT` is now commented and won't be used.

```Dockerfile
# test app
FROM build AS test
WORKDIR /source/tests
RUN dotnet test --logger:trx
#ENTRYPOINT ["dotnet", "test", "--logger:trx"]
```

The following example demonstrates building the Dockerfile with the `RUN dotnet test` line enabled.

```console
MacBook-Pro:complexapp rich$ docker build --pull -t complexapp .
Sending build context to Docker daemon  5.004MB
Step 1/22 : FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
3.0: Pulling from dotnet/core/sdk
Digest: sha256:66f93fea229f812f496441baf0877dd97d9e0c7fb2af209a1b055bcf4bb3919c
Status: Image is up to date for mcr.microsoft.com/dotnet/core/sdk:3.0
<snip>
Step 16/22 : RUN dotnet test --logger:trx
 ---> Running in f2fc51236957
Test run for /source/tests/bin/Debug/netcoreapp3.0/tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_f2fc51236957_2019-10-21_23_45_47.trx

Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 0.9205 Seconds
Removing intermediate container f2fc51236957
 ---> bd74b084d8e8
<snip>
acBook-Pro:complexapp rich$ docker run complexapp
string: The quick brown fox jumps over the lazy dog
reversed: god yzal eht revo spmuj xof nworb kciuq ehT
```

Even though the container image was built, it is still possible to get the logs from inside the intermediate container image. The intermediate stage container image is listed as `bd74b084d8e8` in the console output. Using that information, we can boot up the container image and copy the log files from it, using the following pattern.

```console
MacBook-Pro:complexapp rich$ docker images | grep bd74b084d8e8
<none>                                          <none>                    bd74b084d8e8        3 minutes ago       912MB
MacBook-Pro:complexapp rich$ docker run --rm -dit --entrypoint tail --name complexapp-test bd74b084d8e8
700a24c54acfe8fac6fc2f90a1eda7b72f99d8921738da26f462e6311e6dd723
MacBook-Pro:complexapp rich$ docker cp complexapp-test:/source/tests/TestResults .
MacBook-Pro:complexapp rich$ docker kill complexapp-test
complexapp-test
MacBook-Pro:complexapp rich$ docker ps | grep complexapp
MacBook-Pro:complexapp rich$ 
MacBook-Pro:complexapp rich$ ls TestResults/
_f2fc51236957_2019-10-21_23_45_47.trx
```

If the tests fail, it is not possible to copy the logs from the intermediate container layer, since such a layer won't exist. This limitation, and the difficulty of copying files out of intermediate layers, demonstrates the weakness of this approach.

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
