# Building and testing multiple projects with Docker

It is easy to build and test multiple projects with Docker by following some basic patterns. Docker comes with some requirements and some useful mechanisms that can help you manage various container-based workflows.

The `complexapp` sample is intended to act as a [very simple](complexapp/Program.cs) "complex application". It is composed of multiple projects, including a test project. It is used to demonstrate various workflows, including testing multi-targeted library projects. Some aspects of this example may not be important for your scenario, and can be ignored.

Simpler workflows are provided at [.NET Docker samples](../README.md).

The instructions assume that you have cloned this repo, have [Docker](https://www.docker.com/products/docker) installed, and have a command prompt open within the `samples/complexapp` directory within the repo.

## Building an image including multiple projects

You can build and run the complexapp using the following commands:

```console
docker build --pull -t complexapp .
docker run --rm complexapp
```

It will restore and build all required projects and produce an application image.

In the case of an application with multiple project dependencies, it may be intuitive to place the Dockerfile beside the project file for the application project, and to assign the build context (the `.` in the `docker build` command above) to that same location. This will not work because project dependencies will not exist within the build context. Instead, in a multi-project solution, the best pattern is to place the Dockerfile at the same location you would place a solution file, which is typically one directory above the application project.

## Running tests

One or more of the projects in your solution might be test projects. It can be useful to include testing within same container-based workflow as the application. This is helpful for at least two reasons:

* Integrating testing into this workflow will ensure that tests run in the same environment as the application.
* Get test feedback to validate images before publishing to a registry and deploying to production.

There are two primary ways to test within the workflow of an application container image:

* Run `dotnet test` as a `RUN` step within the image build.
* Expose an opt-in `ENTRYPOINT` as part of a Dockerfile stage.

This is different than running tests within a [.NET SDK container](../run-tests-in-sdk-container.md), which establishes a generic environment (which also works well). The rest of this document is focused on running tests within the same container environment as the application.

## Running tests as an opt-in stage

The benefit of using an opt-in stage for testing is that it enables using the same environment as the build, allows volume mounting (which isn't possible with `docker build`) to collect test logs, and is opt-in so you can skip running tests if you don't want to pay that cost. The downside is that you need to orchestrate building and testing with some form of script. One `docker` command cannot build and test with this model.

The [Dockerfile](Dockerfile) includes two `test` stages that demonstrates running tests via an `ENTRYPOINT`, as follows.

```Dockerfile
# test stage for .NET 5.0 -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test
WORKDIR /source/tests
COPY tests/ .
RUN dotnet build -c debug -f net5.0
ENTRYPOINT ["dotnet", "test", "-f", "net5.0", "--logger:trx"]
```

Note: The two test stages, and testing in terms of a framework, are present due to one of the libraries and the test project being multi-targeted. These are artificial complication in order to demonstrate different (optional) scenarios. If you don't have this need, then you can remove the second test stage.

The presence of the `test` stage costs very little and doesn't change the behavior of the build if you don't specifically target it. By default, the test stage `ENTRYPOINT` will not be used if you build this Dockerfile

The following example demonstrates targeting the `test` stage with the `--target` argument, and with logging enabled, in WSL2:

```console
$ pwd
/home/rich/git/dotnet-docker/samples/complexapp
$ docker build --pull --target test -t complexapp:test .
$ mkdir TestResults
$ docker run --rm -v $(pwd)/TestResults:/source/tests/TestResults complexapp:test
  libfoo -> /source/libfoo/bin/Debug/net5.0/libfoo.dll
  libbar -> /source/libbar/bin/Debug/netstandard2.0/libbar.dll
  tests -> /source/tests/bin/Debug/net5.0/tests.dll
Test run for /source/tests/bin/Debug/net5.0/tests.dll (.NETCoreApp,Version=v5.0)
Microsoft (R) Test Execution Command Line Tool Version 16.9.1
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_512cd4682573_2021-03-30_18_47_24.trx

Passed!  - Failed:     0, Passed:     3, Skipped:     0, Total:     3, Duration: 2 ms - /source/tests/bin/Debug/net5.0/tests.dll (net5.0)
$ ls TestResults/
_512cd4682573_2021-03-30_18_47_24.trx
```

You can do the same thing with the `test31` stage, to test the library with .NET Core 3.1.

```bash
$ docker build --pull --target test31 -t complexapp:test31 .
$ docker run --rm -v $(pwd)/TestResults:/source/tests/TestResults complexapp:test31
Test run for /source/tests/bin/Debug/netcoreapp3.1/tests.dll(.NETCoreApp,Version=v3.1)
Microsoft (R) Test Execution Command Line Tool Version 16.7.1
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_eb8b8c6b50ca_2021-03-30_18_48_30.trx

Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 0.7896 Seconds
$ ls TestResults/
_512cd4682573_2021-03-30_18_47_24.trx  _eb8b8c6b50ca_2021-03-30_18_48_30.trx
```

To sum up this option, if you wanted to build an image, and then test it for multiple target frameworks, your script would look the following, presumably including error checking after each command. If your tests were not multi-targeted, then the last two commands could be removed.

```bash
docker build --pull -t complexapp .
docker build --pull -t complexapp:test --target test .
docker run --rm -v $(pwd)/TestResults:/source/tests/TestResults complexapp:test
docker build --pull -t complexapp:test31 --target test31 .
docker run --rm -v $(pwd)/TestResults:/source/tests/TestResults complexapp:test31
```

## Running tests while building an image

It is possible to run tests as part of `docker build`. This approach can be useful if you want `docker build` to fail if your tests fail.

```Dockerfile
WORKDIR /source/tests
RUN dotnet restore
RUN dotnet test --no-restore --logger:trx
```

Note: Multi-targeted tests present a problem with this scenario since it isn't possibly to use two .NET SDK images in the same `Dockerfiile`, as was done in the previous section. With this model, you either need to not multi-target tests, run the tests with the matching target framework as the .NET SDK image supports, or install a second runtime manually.

The following example demonstrates building with this pattern, as if it was part of [Dockerfile](Dockerfile). You would then be able run tests using the normal `docker build` pattern, as follows:

```console
docker build --pull -t complexapp .
```

You will see that tests are run while building the image, as you can see in the following example (many build steps have been cut out to make the log easier to read).

```console
> docker build --pull -t complexapp .
Sending build context to Docker daemon  12.79MB
Step 1/24 : FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
5.0: Pulling from dotnet/sdk
Digest: sha256:02606610ffd96978da91adeac1b73af9ac536b85a3034b061d0c7d8d9fcd6790
Status: Image is up to date for mcr.microsoft.com/dotnet/sdk:5.0
 ---> 9817c25953a8
Step 17/24 : RUN dotnet test --logger:trx
 ---> Running in 4678e2e6456d
  Determining projects to restore...
  Restored /source/tests/tests.csproj (in 7.73 sec).
  2 of 3 projects are up-to-date for restore.
  libbar -> /source/libbar/bin/Debug/netstandard2.0/libbar.dll
  libfoo -> /source/libfoo/bin/Debug/netstandard2.0/libfoo.dll
  tests -> /source/tests/bin/Debug/net5.0/tests.dll
Test run for /source/tests/bin/Debug/net5.0/tests.dll (.NETCoreApp,Version=v5.0)
Microsoft (R) Test Execution Command Line Tool Version 16.8.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_886d04dbf347_2020-11-02_18_30_59.trx

Passed!  - Failed:     0, Passed:     2, Skipped:     0, Total:     2, Duration: 2 ms - /source/tests/bin/Debug/net5.0/tests.dll (net5.0)
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

12/19/2019  02:41 PM    <DIR>          .
12/19/2019  02:41 PM    <DIR>          ..
12/19/2019  02:41 PM             3,635 _4678e2e6456d_2019-12-19_22_41_08.trx
```

There are two problems with this approach. It is cumbersome and if tests fail, it is not possible to copy the logs from the intermediate container layer, since that layer won't exist. This limitation, and the difficulty of copying files out of intermediate layers, demonstrates the weakness of this approach.

Alternatively, you can build with the `--rm=false` option. This leaves the intermediate build containers and you can then issue the `docker cp` command to retrieve the results. The use of docker system prune makes it easy to cleanup the intermediate build containers.

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/samples/README.md)
