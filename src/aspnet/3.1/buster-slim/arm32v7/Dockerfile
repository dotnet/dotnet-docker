ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.28-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.28 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='086f7ed75527807b9701e9edb9f12672ed603cd8bce05c76bd4fd7cb4a0d09130134570fa4dd82d2f47f989941e63b3e8cc2ae30eb256d7f9a3a41bc861c6f32' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
