ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.11-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.2.20155.1 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='4cc3098a73431ff2898535a097d28284859cf0dfb836bbf2e3eccd767a225312b2e41c3190cf27adb447d6b4a24160da56ad20bf0397bb5ea3b1ece54051b987' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
