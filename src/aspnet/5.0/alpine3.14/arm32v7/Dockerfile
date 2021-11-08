ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.12-alpine3.14-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.12

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='16222fd605b91bd372e3b5f8dc7b8bfd2dc624a2d47b4032f93f5ac1c88da42e5011ee2387db392e889165a10f2ed55ed8f53c8e434638f8d2f04ca6d5c42b9a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
