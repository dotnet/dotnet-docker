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

This is the approach used with [complexapp](.). The [Dockerfile](./Dockerfile) for the sample is at the same location as the `.sln` file, and all projects are available when that same location is used as the build context. There are other options, but this approach is the easiest.

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

> [!NOTE]
> See [Establishing docker environment](../establishing-docker-environment.md) for more information on correctly configuring Dockerfiles and `docker build` commands.

## Running tests

There are many ways to run automated tests against your projects.
[Running Tests with Docker](../run-tests-in-sdk-container.md) is one example that covers running this sample's tests using the separate executable test stage in the [`complexapp` Dockerfile](./Dockerfile).

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker/blob/main/samples/README.md)
