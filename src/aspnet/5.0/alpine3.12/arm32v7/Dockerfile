ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.1

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='3b14a029d3537e10a90eb3bfd1a895a8d57aa44c907275130f58605d18120905c8a88c840a6a3962ed8d1329576be79a4a0033a86c9052988613ab10a0f95ae1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
