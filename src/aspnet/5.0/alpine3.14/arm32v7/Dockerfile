ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.11-alpine3.14-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.11

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='ce42c7ca6f5fc3cc9169df710393a4a9b9919d1b7e20907b4e0028a7fbbb0baa70210d12f6bfc2d05505b0b8d7111ff323e5df2a71ea4892026bd0940f9ae671' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
