# Testing a multi-targeted library in containers

Containers are a convenient system for testing. They enable reliably and cheaply establishing the desired test environment. [Building and testing multiple projects with Docker](../complexapp/README.md) describes how to run unit tests in the same environment as the overall application build. This document describes how to establish multiple different .NET environments in containers to validate the various targets of a multi-targeted library.

The primary mechanism for testing a [multi-targeted library](MovingAverage) is creating, targeting, and running multiple test stages that rely on different versions of the .NET SDK.

## Copying source

The source is copied into a `scratch` based stage. The advantage of this approach is that it doesn't depend on a specific image that may not be the right one.

```Dockerfile
FROM scratch AS source
WORKDIR /source

# copy source
COPY . .
```

## Test stages

Each test stage depends on a specific SDK version, copies source from the `source` stage, builds the tests and dependencies for a given target framework, and then sets the entry point for running tests.

```Dockerfile
# test stage -- target with: docker build --target test60
FROM mcr.microsoft.com/dotnet/nightly/sdk:6.0 AS test60
COPY --from=source /source /source
WORKDIR /source/tests
RUN dotnet build -f net6.0
ENTRYPOINT ["dotnet", "test", "-f", "net6.0", "--no-build", "--logger:trx"]
```

The number `60` in `test60` is shorthand for *test stage for `net6.0` target framework.* The same scheme is used for .NET Core 3.1.

## Building a stage

You can build a test stage with the `--target` argument.

```bash
docker build --target test60 -t movingaverage60 .
```

## Scripting tests

You can script testing multiple target frameworks for a given multi-targeted library.

```bash
docker build --target test60 -t movingaverage60 .
docker run --rm movingaverage60
docker build --target test31 -t movingaverage31 .
docker run --rm movingaverage31
```

If you want to harvest the test logs, you can adapt the `docker run` commands to volume mount a `TestResults` directory, as follows.


```bash
docker build --target test60 -t movingaverage60 .
docker run --rm -v $pwd\TestResults:/source/tests/TestResults movingaverage60
docker build --target test31 -t movingaverage31 .
docker run --rm -v $pwd\TestResults:/source/tests/TestResults movingaverage31
```
