ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.36-alpine3.20-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.36

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://builds-int.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='0f772735acac7258c2af4a67c881d5afac2f841f9ab691a06ba1e34abd81d5831d09fe8886e025f7d1fbe84a1e93415fb8e35511c4bb988dd94cb823c18d0078' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
