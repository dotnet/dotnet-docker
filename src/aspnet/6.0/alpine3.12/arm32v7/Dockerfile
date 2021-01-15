ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.21064.29

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='6685010d6b0b46da2d6f0ba24b1c1e6b7dd53566ebda765f9bb1c8290917841e0ce54123bafc008a4858c2f106b3a9d665d39dbb0a72ac4853a73ee6a8e42576' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
