ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.11-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.2 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='cfe565cd1bf112425decf9a4a9be0bf0a71c7a4180b9134a4103360ddcd0cf4ef166cab4327d57f72f5113edace6a2a9a6dba25b914d439ddb6018d96985a2cb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
