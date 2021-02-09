ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-bionic

# Install ASP.NET Core
RUN aspnetcore_version=3.1.12 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='e6d384a4c05bc6a693a85dc1da5f34e26449ad5d9414dee5f46a56805ac53eb304610be06d6a2a683f2d9e1447f316f47abea71fbfd6ee901dcc9da9d7c4e03b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
