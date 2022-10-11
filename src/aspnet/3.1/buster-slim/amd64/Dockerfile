ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.30-buster-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.30 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='afae1f5ab022b81636a0d6fe3956d491c3f28206f8177787013f309841dcb9f1134b33677a9cf3fd68a5c86cff7fcb0694eb597dc99a96dacd704e89120375a7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
