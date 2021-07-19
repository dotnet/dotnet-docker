ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.14-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21365.18 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='043b8311a14584d6c06ce19bcca5cf2ba0f4c83b4173d8599dfb50bc7e5adfb649cbdc3942db82d7758ef36f5b85a45cbdcc6b648a00e9ec3aad4ade2181bda0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
