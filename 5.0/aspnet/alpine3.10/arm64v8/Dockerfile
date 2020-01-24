ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.10-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-alpha.1.20071.6 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='c2cc8d36636d51485184b3d654c6a5500e8123a089b0242ee1d395c01d80b299dff9c139873eff3b06d8b536eeef76591b5d7141d2a30b36c9521f11e2137cf0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
