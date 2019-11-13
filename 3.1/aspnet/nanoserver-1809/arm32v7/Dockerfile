# escape=`

ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-nanoserver-1809-arm32v7

# Install ASP.NET Core Runtime
ENV ASPNETCORE_VERSION 3.1.0-preview3.19555.2

RUN curl -SL --output dotnet.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/%ASPNETCORE_VERSION%/aspnetcore-runtime-%ASPNETCORE_VERSION%-win-arm.zip `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" ./shared/Microsoft.AspNetCore.App `
    && del dotnet.zip
