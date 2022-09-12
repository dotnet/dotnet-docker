ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.29-alpine3.16-arm64v8

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
RUN aspnetcore_version=3.1.29 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='36889945d974d440dc55e6631a61b19f6a6b6b6513cbd92f44ecdcda05e1215c61b6b2ae5ab0d1e74d8c7bbe5418f76c6dae81cbd53581f00fa02a6394aeebf6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
