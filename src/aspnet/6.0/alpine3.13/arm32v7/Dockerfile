ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-rc.1-alpine3.13-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21411.15 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='6e57d49ff4f518b0ef0166e03bb326b9623cc9133ec0023adef4b5cb0c58f27af45116aef71098f1b423f5949d911f5afab16a3ee64ee5db2a48083460ae0b7e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
