ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.15-alpine3.14-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.15

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='33ee1577c68a41aa9a0595e52a97e76bb389a5bbc573afd70d25244c8f908b3d0080dde5151fd7f29d4560c84355bcaeb1c83523bf80cbc8fc099e5d51683519' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
