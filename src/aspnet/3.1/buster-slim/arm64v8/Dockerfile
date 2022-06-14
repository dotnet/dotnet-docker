ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.26-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.26 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='757ff6cbc31b1c8743077288d7fa621c73fa7f4d155d636ad100cda6e1f601e31d2f842d5cfef3dec5daa4c8c3efbcf76f02afd1c518cae7b67b2a46a9faab08' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
