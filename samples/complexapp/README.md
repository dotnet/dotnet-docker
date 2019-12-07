# Building and testing multiple projects with Docker

It is easy to build and test multiple .NET projects with Docker by following some basic patterns. Docker comes with some requirements and some useful mechanisms that can help you manage various container-based workflows.

The `complexapp` sample is intended to act as a very simple version of a complex application that has multiple projects, including a test project.

The instructions assume that you have cloned this repo, have [Docker](https://www.docker.com/products/docker) installed, and have a command prompt open within the `samples/complexapp` directory within the repo.

## Building an image for an application with multiple projects

The most common way to build images is using following pattern:

```console
docker build -t tag .`
```

The most important aspect to discuss is the `.` at the end of the command. It represents the path to the build context, where docker will look for assets that are referenced in the Dockerfile and for a `Dockerfile` (if not explicitly specified). The build context is packaged up and then sent to the docker daemon (the server), which performs the actual build. As a result, it is critical that all required assets are available within the build context, otherwise, they will not be available while the image is being built, which would produce an error.

In the case of an application with multiple project dependencies, it may be intuitive to place the Dockerfile beside the project file for the application project. This will not work because project dependencies will not exist within that context.

Instead, in a multi-project solution, the best pattern is to place the Dockerfile at the same location you would place a solution file, which is typically one directory above the application project. At that location, you can use the same pattern that is demonstrated above, using `.` for the build context and relying on the Docker client to automatically locate the Dockerfile. 

This is the approach used with [complexapp](.). The Dockerfile for the sample is at the same location as the `.sln` file, and all projects are available when that same location is used as the build context. There are other options, but this approach is the easiest.

You can build and run the complexapp using the following commands:

```console
docker build --pull -t complexapp .
docker run --rm complexapp
```

## Running tests within the application container

It can be useful to include testing within a container-based workflow. This is helpful for at least two reasons: 

* Integrating testing into this workflow will ensure that tests run in the same environment as the application.
* It is useful to get early test feedback before publishing an image to a registry and letting it go to production.

The `Dockerfile` contains the following section that is dedicated to testing:

```Dockerfile
# test app
FROM build AS test
WORKDIR /source/tests

# Uncomment out next 'RUN' line if you want to run tests as part of docker build
# and for test failure to cascade to build failure
# otherwise, you can build and run 'test' stage using the test ENTRYPOINT
#RUN dotnet test --logger:trx
ENTRYPOINT ["dotnet", "test", "--logger:trx"]
```

By default, this section does very little and has very little cost. It allows you to build the image in a kind of "test mode" by using the `--target` argument to build a the `test` stage. The following example demonstrates doing that.

```console
docker build --pull -t complexapp:test --target test .
docker run --rm complexapp:test
```

You can see this in action in the following example:

```console
% docker run --rm complexapp:test            
Test run for /source/tests/bin/Debug/netcoreapp3.1/tests.dll(.NETCoreApp,Version=v3.1)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_22d322c828dd_2019-11-17_21_45_42.trx

Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 0.9783 Seconds
```

The test runner saves a log of the tests that can be read and processed. By default, this file is saved within the image, but can be saved to the host file system via volume mounting. See [Running .NET Core Unit Tests with Docker](../unit-testing-in-container.md) for more information on how to adopt that pattern.

For example, the following pattern (here, using Linux containers on Windows) will capture the test results into a test results directory.

```console
mkdir TestResults
docker build --pull --target test -t complexapp-test .
docker run --rm -v %cd%/TestResults:/source/tests/TestResults complexapp-test
```

## Running tests while building an image

You can also run unit tests as part of `docker build`, without using stages. This can work well if your goal is to simply fail building an image if tests fail. It isn't possible to volume mount as part of the build command, which prevents you from saving the log file to the host machine. If you want to adopt this pattern, then you need to uncomment out the following line:

```console
#RUN dotnet test
```

You can then build using the normal pattern, as follows:

```console
docker build --pull -t complexapp .
```

You will see that tests are run while building the image.

You may consider leaving the `ENTRYPOINT` line as-is (not commenting it out). It will enable you to re-run tests in the way demonstrated above, to enable you to save test logs to the host (in case your tests fail).

## Resources

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/master/samples/README.md)
