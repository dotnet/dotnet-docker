ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.4 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='58fe16baf370cebda96b93735be9bc57cf9a846b56ecbdc3c745c83399ad5b59518251996b75ac959ee3a8eb438a92e2ea3d088af4f0631caed3c86006d4ed2d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
