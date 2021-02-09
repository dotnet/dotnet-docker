ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-alpine3.13-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.12 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='9f8c9fd651ab0784d9449b0f314575f7c3cf3473235fb9d1ea505b970e498c0ff5bd94c347c59e5d2859454e95ea266182e3ba1402fd323909f4f0c382fa0260' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
