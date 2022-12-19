# Using the System.Drawing.Common Package in a Docker Container

If your application makes use of the System.Drawing.Common NuGet package, you'll need to ensure some additional dependencies are installed in your Docker container. See #1098 for more info on this issue.

> NOTE: [The System.Drawing.Common package is not supported on Linux](https://learn.microsoft.com/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only).

The required dependencies of of the System.Drawing.Common NuGet package are not available in Windows Nano Server. So you'll need to use Windows Server Core instead. Official .NET images for Windows Server Core are available with tags documented on [Docker Hub](https://hub.docker.com/_/microsoft-dotnet). Example tag: `mcr.microsoft.com/dotnet/aspnet:7.0-windowsservercore-ltsc2022`.
