ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.28-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.28 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='28371a6888d41e938f14cceb6c8a4471a1f0d7b585045e65b914f23ffb894f72a66a4a4cdaaf6d21dfd60063bb35d88c36fce8d4411a89c89b52023807639f82' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
