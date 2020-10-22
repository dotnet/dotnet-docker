ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rtm.20517.7

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='93bc8854bb28533148353efb398010923ff3e7101eff3f2a97ed4c4f6e3fb99323dd9d6cd12674f83979ba96ffb695e8290c7d8fbfaf4b1987a6a46945a03531' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
