ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.0 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='6244dd1dd7d18f00bcc6a1faa2b6da61bc3edc9748f9513b617c84b1c2cb04d3b7328f398ce9a1e9834db56bb6246756b7c5409c179000eaab113a88a2da5b3a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
