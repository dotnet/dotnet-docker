ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.11

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.5.20272.1

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='193eef956574cc70931a4d2e049e288c9dc6d70a5d325f775e52c4ca12af320e4f95fc7d1ee8cbf0dd05792cbd2d4279fad1caaaf196c9f85891bb708bc0aa41' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
