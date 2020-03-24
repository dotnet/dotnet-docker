ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.11-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.3 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='57f81e61e7e16b91a2f9f5ae5978efb3ee31cd4056d7f2e6ca07da1ef991ec070d54a99a0ba856affb54defc625276980da683d8d5fab70de0ac53631385fc75' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
