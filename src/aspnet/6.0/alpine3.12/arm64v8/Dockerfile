ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.21067.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='6ca2c17dfd49fcf63ec5d3d503a545ce7c5959efc0f70b1f7ef602d787965073cc0e196acdc6cf2e52a410829aa93a5e23481a412b98a78ea17224a84f9469a2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
