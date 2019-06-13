ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.9-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19311.5

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='596d0e6d9428504ae0dd41294a9175f70c76726a8df252c8e68b0a047bcee7dfa5e57c1324177725d00d2baba6dc3a81e5c7e9c9ebdefdb0dae7eeec642f6f2b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
