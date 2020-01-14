# escape=`

ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-nanoserver-1809-arm32v7

# Install ASP.NET Core Runtime
RUN set "aspnetcore_version=3.1.1" `
    && call curl -SL --output dotnet.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/%aspnetcore_version%/aspnetcore-runtime-%aspnetcore_version%-win-arm.zip `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" ./shared/Microsoft.AspNetCore.App `
    && del dotnet.zip
