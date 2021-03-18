ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21167.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='21112bddcf3bdebee6a2529c510ac331147287ec47df211d5f6d30d8c74afe5a1893be298126881f1a5d7e4cd5a88f7de40aecde81f4ee51b5cb2f58c0552c2d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
