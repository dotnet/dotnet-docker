ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.0-preview.6-alpine3.18-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23327.8

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='8fd1c1177a88d8d907207e5808105ecb094ae7d9e6558456d6e85b080f37365ea164f3bd9086d652874fca8d613a9054323ca839ca4cfae2596ba10901094300' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
