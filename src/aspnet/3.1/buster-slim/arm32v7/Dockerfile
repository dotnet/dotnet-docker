ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.26-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.26 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='ae09b848d6a550ac1c04763de2b1b8bdd1860f0fe3c45278cd2fcaa700e54379ae09500ec917c36cb24b4b19f67629d08faaccba39ebf2e2b0cf58db2db253b3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
