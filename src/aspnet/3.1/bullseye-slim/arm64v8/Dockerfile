ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.30-bullseye-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.30 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='327116926ed9d4a86664c6d3687d59261353639b67beafa8d451d8546eb800804ace64d40a05e14db5dc6ec638fc041efbd209ee58430fb539d02799c1a33c55' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
