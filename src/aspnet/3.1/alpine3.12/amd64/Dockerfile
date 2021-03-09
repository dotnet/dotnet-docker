ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-alpine3.12

# Install ASP.NET Core
RUN aspnetcore_version=3.1.13 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='418b18bfd3a5e03ba0129720eef361fae6ba001263a0ec72b4cc018b8a6b90c8215df1ffae26c429e5a594c4425275996454666e6e0f2d66efffa6c844ee1a1a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
