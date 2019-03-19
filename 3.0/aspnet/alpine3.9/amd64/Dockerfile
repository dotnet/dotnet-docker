ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.9

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview4-19168-01

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='3aa56c0e77d8ced59ab9cbf6c26d12c7af1918cf9d555875b65ab3bf2e10ee629a6ff3d37bbffeb72e9b773986ee4bc991138d1bbb810b910ed67457cda21b2c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
