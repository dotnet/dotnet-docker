# .NET Core and Docker for ARM64

We are in the process of enabling .NET Core on ARM64, including with Docker. See [dotnet/dotnet-docker # 509](https://github.com/dotnet/dotnet-docker/pull/509) to follow the progress.

Please see [.NET Core and Docker for ARM32](dotnet-docker-arm64.md) if you are interested in [ARM32](https://en.wikipedia.org/wiki/ARM_architecture) usage.

You can run the ARM32 build of .NET Core on an ARM64 machine. You need to install the `armhf` versions of .NET Core's dependent packages for that to work. The dependent packages are documented in the `runtime-deps` [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/stretch-slim/arm32v7/Dockerfile) for each distro.
