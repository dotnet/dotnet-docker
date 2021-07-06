ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-preview.5-alpine3.13-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21328.3 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='d493e4c43f972d29963b1eea70c92cf41f67ac9f3ba76151d3d97d429737858f667cb4b5a91f57c8d88a94540eaac0968cd24e3fbdd2c5e323389fa286c96913' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
