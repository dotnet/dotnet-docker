# Using the System.Drawing.Common Package in a Docker Container

The required dependencies of the System.Drawing.Common NuGet package are only [available in Windows Server Core and full Server containers](https://learn.microsoft.com/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only). Official .NET images for Windows Server Core are available with tags documented on [Docker Hub](https://hub.docker.com/_/microsoft-dotnet). Example tag: `mcr.microsoft.com/dotnet/aspnet:7.0-windowsservercore-ltsc2022`.
