ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.0-preview.6-alpine3.18-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23320.14

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='9d54c2646400968b6d1962dc06808edae8ab3cf75f6d15f5311e801de392bb49355dba435a26a0514de1d221da938a3da61cb0ff844251a65e52a231d6f0046b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
