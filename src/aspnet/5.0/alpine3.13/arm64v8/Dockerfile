ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.12-alpine3.13-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.12

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='f7a0d0462f9062e79a4dcafa9b3c4161cb7e44b137a515f93b98dee7cdfcf9e36d1dad6f4e6488c3aaaf0974cfa4b55f25571119f97324cb987d0eece1db83ae' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
