# escape=`

ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-nanoserver-1809-arm32v7

# Install ASP.NET Core Runtime
RUN set "aspnetcore_version=5.0.0-preview.2.20167.3" `
    && call curl -SL --output dotnet.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/%aspnetcore_version%/aspnetcore-runtime-%aspnetcore_version%-win-arm.zip `
    && tar -zxf dotnet.zip -C "%ProgramFiles%\dotnet" ./shared/Microsoft.AspNetCore.App `
    && del dotnet.zip
