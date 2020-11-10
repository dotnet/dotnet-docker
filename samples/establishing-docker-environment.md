# Establishing a Docker environment

Docker establishes a virtualized environment that separates the guest (the container) from the host (your computer or production server). This separation is the definition of virtualization. When you establish a Docker environment, you specify the settings and files from the host that you want the guest to have access to.

Programming environments, like .NET, have affordances for looking for assets and settings in multiple locations. They also may look at multiple types of files (like `.sln`, `.cs` and `.csproj` files). These locations and file types will not naturally be accessible from within a Docker environment. When you define a Docker environment, you need to intentionally specify all the files, locations and settings that are required for the operation and outcome you want.

The remainder of this document will consider the various configuration options used by [.NET samples](README.md).

The [dotnetapp sample](dotnetapp/README.md) demonstrates establishing a Docker environment for a single directory of assets.

The [Dockerfile](dotnetapp/Dockerfile) includes the following lines:

```Dockerfile
COPY *.csproj .
RUN dotnet restore
```

These lines copy any C# `.csproj` project files in the current directory and then restore them.

The dotnetapp sample specifies the following `docker build` command to build an image:

```console
docker build --pull -t dotnetapp .
```

The `.` at the end of the command resolves to the current directory and establishes the part of the file system that `docker build` has access to. Another directory could also be specified. In the case of the dotnetapp sample, the `.` directory contains all required assets, both the project file and all source files (in this case, a single one).

The following cases would require a different approach:

* A `Directory.build.props` file exists that specifies additional settings. In this case, another `COPY` command is required for `*.props` files.
* A `nuget.config` file exists that specifies additional feeds. In this case, another `COPY` command is required for `nuget.config`.
* The `.csproj` file(s) has dependencies. Dependent `.csproj` files must also be copied before running `dotnet restore`, otherwise the command will fail.

The [complexapp](complexapp) sample contains multiple project files. The instructions require that the the `docker build` path is at the location that contains all project files, at [complexapp](complexapp). Each of the project files is stored within a child directory within [complexapp](complexapp).
